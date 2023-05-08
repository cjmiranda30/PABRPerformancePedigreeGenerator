using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form16 : Form
    {
        public string fn = string.Empty;
        public Form16()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Reset details?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                foreach (var control in groupBox1.Controls)
                {
                    if (control is System.Windows.Forms.TextBox textBox)
                    {
                        textBox.Text = string.Empty;
                    }
                }

                pictureBox1.Image = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isNullOrEmpty = pictureBox1 == null || pictureBox1.Image == null;

            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                textBox1.Focus();
                MessageBox.Show("Staff name is required.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
            {
                textBox3.Focus();
                MessageBox.Show("Staff position is required.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isNullOrEmpty)
            {
                button3.Focus();
                MessageBox.Show("Upload Staff Picture.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isInserted = AddStaff();

            if (isInserted)
            {
                MessageBox.Show("Staff Added.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to add staff.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Dispose();
        }

        private bool AddStaff()
        {
            bool res = false, uploadSuccess = false;
            string fileName = string.Empty;
            bool isNullOrEmpty = pictureBox1 == null || pictureBox1.Image == null;

            if (!isNullOrEmpty)
            {
                //Upload Event Poster Picture to get filename
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://pabrdexapi.com");
                    //client.BaseAddress = new Uri("https://localhost:7060/");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);

                    using (var content = new MultipartFormDataContent())
                    {

                        // Convert the image to a byte array
                        byte[] imageData;
                        using (var ms = new MemoryStream())
                        {
                            pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                            imageData = ms.ToArray();
                        }

                        // Add the image data to the request body as a ByteArrayContent
                        var imageContent = new ByteArrayContent(imageData);

                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(imageContent, "file", Path.GetFileName(fn));

                        // Make the POST request to the web API endpoint
                        //var response = client.PostAsync("api/PedigreeChart/upload-dog-picture", content).Result;
                        var response = client.PostAsync("api/ContentData/upload-content-image", content).Result;

                        var resp = response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                        var status = responseJson.status;
                        var title = responseJson.title;


                        // Check the response status code and handle any errors
                        if (status == "error")
                        {
                            uploadSuccess = false;
                        }
                        else if (status == "success" && title == "Uploaded Successfully")
                        {
                            var imgName = responseJson.imgName;
                            fileName = imgName;
                            uploadSuccess = true;
                        }
                    }
                }
            }

            //Insert event poster to DB
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var dataParams = new
                {
                    name = textBox1.Text.Trim(),
                    position = textBox3.Text.Trim(),
                    twitter = textBox4.Text.Trim(),
                    facebook = textBox2.Text.Trim(),
                    instagram = textBox5.Text.Trim(),
                    image = fileName
                };
                var json = JsonConvert.SerializeObject(dataParams);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/ContentData/InsertPABRStaff", content).Result;

                var resp = response.Content.ReadAsStringAsync();

                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error" && title == "Data Not Added")
                {
                    res = false;
                }
                else if (status == "success" && title == "Data Added")
                {
                    res = true;
                    var msg = responseJson.message;
                }
            }

            return res;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                pictureBox1.Image = new Bitmap(open.FileName);
                fn = open.FileName;

            }
        }
    }
}

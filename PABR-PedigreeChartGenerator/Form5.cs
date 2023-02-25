using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form5 : Form
    {
        public string fn = string.Empty;
        public Form5()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isNullOrEmpty = pictureBox1 == null || pictureBox1.Image == null;
            bool allTextboxesFilled = true;
            TextBox firstEmptyTextBox = null;

            foreach (var control in this.groupBox1.Controls)
            {
                if (control is TextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        allTextboxesFilled = false;
                        firstEmptyTextBox ??= textBox;
                    }
                }
            }

            if (!allTextboxesFilled)
            {
                firstEmptyTextBox.Focus();
                MessageBox.Show("Don't leave any blank.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isNullOrEmpty)
            {
                button3.Focus();
                MessageBox.Show("Upload dog picture.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isInserted = AddDogDetails();

            if (isInserted)
            {
                MessageBox.Show("Dog registered.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Hide();
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Reset details?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                foreach (var control in groupBox1.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Text = string.Empty;
                    }
                }

                pictureBox1.Image = null;
            }
        }

        private bool AddDogDetails()
        {
            bool res = false, uploadSuccess = false;
            string fileName = string.Empty;

            //Upload Dog Picture to get filename
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                //client.BaseAddress = new Uri("https://localhost:7060/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);

                using (var content = new MultipartFormDataContent())
                {
                    // Convert the image to a byte array
                    //byte[] imageData;
                    //using (var ms = new MemoryStream())
                    //{
                    //    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                    //    imageData = ms.ToArray();
                    //}

                    //// Add the image data to the request body as a ByteArrayContent
                    //var imageContent = new ByteArrayContent(imageData);
                    //content.Add(imageContent, "file", fn);

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
                    var response = client.PostAsync("api/PedigreeChart/upload-dog-picture", content).Result;

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
            //Insert to DB
            if (uploadSuccess)
            {

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var dogParams = new
                    {
                        dogName = textBox1.Text.Trim(),
                        gender = textBox2.Text.Trim(),
                        breed = textBox3.Text.Trim(),
                        color = textBox4.Text.Trim(),
                        ownerName = textBox5.Text.Trim(),
                        pabrNo = textBox6.Text.Trim(),
                        registryNo = textBox7.Text.Trim(),
                        picURL = fileName
                    };
                    var json = JsonConvert.SerializeObject(dogParams);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = httpClient.PostAsync("api/PedigreeChart/AddDog", content).Result;

                    var resp = response.Content.ReadAsStringAsync();

                    var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                    var status = responseJson.status;
                    var title = responseJson.title;

                    if (status == "error" && title == "Dog Not Registered")
                    {
                        res = false;
                    }
                    else if (status == "success" && title == "Dog Registered")
                    {
                        res = true;
                        var msg = responseJson.message;
                    }
                }
            }
            else
            {
                res = false;
            }

            return res;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form4 : Form
    {
        private bool isPicChanged = false;
        private bool isPicRemoved = false;
        public string fn = string.Empty;
        public Form4()
        {
            InitializeComponent();
            loadDogDetails();
        }
        private async void loadDogDetails()
        {

            textBox1.Text = CurSelectedDog.DogName.Trim();
            comboBox1.SelectedIndex= (CurSelectedDog.Gender.Trim() == "M") ? 0: 1;
            textBox2.Text = CurSelectedDog.Breed.Trim();
            textBox3.Text = CurSelectedDog.Color.Trim();
            textBox4.Text = CurSelectedDog.OwnerName.Trim();
            textBox5.Text = CurSelectedDog.PABRno.Trim();
            label8.Text = label8.Text + " " + CurSelectedDog.DateAdded.Trim();

            try
            {
                DateTime dateValue = DateTime.ParseExact(CurSelectedDog.DoB, "MM-dd-yyyy", null);
                dateTimePicker1.Value = dateValue;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "MMMM d, yyyy";
            }
            catch
            {
            }

            if (!string.IsNullOrWhiteSpace(CurSelectedDog.PicURL))
            {
                pictureBox1.Load("https://pabrdex.com/images/" + CurSelectedDog.PicURL);
                label7.Text = string.Empty;
                button4.Visible = true;
            }
            else
            {
                label7.Text = "No uploaded picture.";
                button4.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete this dog?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.DeleteAsync("api/PedigreeChart/DeleteDog?ID=" + CurSelectedDog.UID).Result;
                    var resp = response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                    var status = responseJson.status;
                    var title = responseJson.title;

                    if (status == "error" && title == "Dog Not Deleted")
                    {
                        MessageBox.Show("Unable to delete dog.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (status == "success" && title == "Dog Deleted")
                    {
                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (System.Windows.Forms.Application.OpenForms["Form3"] != null)
                        {
                            //(System.Windows.Forms.Application.OpenForms["Form3"] as Form3).LoadDataGridView();
                            Form3.DeleteDogToTable(CurSelectedDog.UID);
                            (System.Windows.Forms.Application.OpenForms["Form3"] as Form3).SearchV3();
                        }
                        this.Dispose();
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text.Trim()))
            {
                MessageBox.Show("PABR Number is required.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult dr = MessageBox.Show("Update details?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                int pDogID = int.Parse(CurSelectedDog.UID);
                string pDogName = string.IsNullOrWhiteSpace(textBox1.Text.Trim()) ? "" : textBox1.Text.Trim(),
                    pGender = (comboBox1.SelectedIndex == 0) ? "M" : "F", pBreed = string.IsNullOrWhiteSpace(textBox2.Text.Trim()) ? "" : textBox2.Text.Trim(),
                    pColor = string.IsNullOrWhiteSpace(textBox3.Text.Trim()) ? "" : textBox3.Text.Trim(),
                    pDoB = dateTimePicker1.Value.ToString("MM-dd-yyyy"),
                    pOwnerName = string.IsNullOrWhiteSpace(textBox4.Text.Trim()) ? "" : textBox4.Text.Trim(),
                    pPABRNo = string.IsNullOrWhiteSpace(textBox5.Text.Trim()) ? "" : textBox5.Text.Trim();

                bool res = UpdateDogDetails(pDogID, pDogName, pGender, pBreed, pColor, pDoB, pOwnerName, pPABRNo);

                if (res)
                    MessageBox.Show("Dog successfully updated!", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("PABR No. already exists!", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (System.Windows.Forms.Application.OpenForms["Form3"] != null)
                {
                    //(System.Windows.Forms.Application.OpenForms["Form3"] as Form3).LoadDataGridView();
                    (System.Windows.Forms.Application.OpenForms["Form3"] as Form3).SearchV3();
                }
                this.Dispose();
            }


        }

        private bool UpdateDogDetails(int DogID, string DogName, string Gender, string Breed, string Color, string DoB, string OwnerName, string PABRNo)
        {
            bool result = false;

            //Update dog details
            string fileName = string.Empty;

            #region Dog Picture
            if (isPicChanged)
            {
                bool isNullOrEmpty = pictureBox1 == null || pictureBox1.Image == null;

                if (!isNullOrEmpty)
                {
                    //Upload Dog Picture to get filename
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
                            var response = client.PostAsync("api/PedigreeChart/upload-dog-picture", content).Result;

                            var resp = response.Content.ReadAsStringAsync();
                            var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                            var status = responseJson.status;
                            var title = responseJson.title;


                            // Check the response status code and handle any errors
                            if (status == "success" && title == "Uploaded Successfully")
                            {
                                var imgName = responseJson.imgName;
                                fileName = imgName;
                            }
                        }
                    }
                }
            }

            if (isPicRemoved)
            {
                fileName = "Removed";
            }
            #endregion

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);

                string qString = string.Empty;

                if (fileName == "")
                {
                    qString = "api/PedigreeChart/UpdateDog?DogID=" + DogID.ToString() + "&DogName=" + DogName + "&Gender=" + Gender + "&Breed=" + Breed + "&Color=" + Color + "&DoB=" + DoB + "&OwnerName=" + OwnerName + "&PABRNo=" + PABRNo;
                }
                else if (fileName == "Removed")
                {
                    //string blnkVal = string.Empty;
                    string blnkVal = "Removed";
                    qString = "api/PedigreeChart/UpdateDog?DogID=" + DogID.ToString() + "&DogName=" + DogName + "&Gender=" + Gender + "&Breed=" + Breed + "&Color=" + Color + "&DoB=" + DoB + "&OwnerName=" + OwnerName + "&PABRNo=" + PABRNo + "&PicURL=" + blnkVal;

                }
                else
                {
                    qString = "api/PedigreeChart/UpdateDog?DogID=" + DogID.ToString() + "&DogName=" + DogName + "&Gender=" + Gender + "&Breed=" + Breed + "&Color=" + Color + "&DoB=" + DoB + "&OwnerName=" + OwnerName + "&PABRNo=" + PABRNo + "&PicURL=" + fileName;
                    
                }

                var response = httpClient.PostAsync(qString, null).Result;

                var resp = response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error")
                {
                    result = false;
                }
                else if (status == "success")
                {
                    result = true;

                    Form3.UpdateDogToTable(DogID.ToString(), DogName, Gender, Breed, Color, DoB, OwnerName, PABRNo, fileName);
                }

            }

            return result;
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
                isPicChanged = true;
                label7.Text = string.Empty;
                button4.Visible = true;
                isPicRemoved = false;
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MMMM d, yyyy";


            bool isNullOrEmpty = pictureBox1 == null || pictureBox1.Image == null;
            if (isNullOrEmpty)
            {
                button4.Visible = false;
            }
            else
            {
                button4.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            label7.Text = "No uploaded picture.";
            button4.Visible = false;
            isPicRemoved = true;
        }
    }
}

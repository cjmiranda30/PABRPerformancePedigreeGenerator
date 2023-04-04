using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Metadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Globalization;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }
        private bool isImageFolderNotAvailable = false;

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a new OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the filter to only allow CSV files
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            // Set the initial directory to the user's My Documents folder
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Show the dialog and wait for the user to select a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the path of the selected file and store it as a string
                string filePath = openFileDialog.FileName;

                // Do something with the file path (e.g. pass it to another function)
                //Console.WriteLine($"Selected file path: {filePath}");
                textBox1.Text = filePath;
                label3.Visible = false;


                string[] accceptedColumns = { "DogName", "Gender", "Breed", "Color", "Owner", "PABRNo", "DOB", "ImageName" };
                string[] lines = File.ReadAllLines(filePath);

                if (lines[0].Split(',').Count() != 8)
                {
                    //Invalid number of columns
                    MessageBox.Show("Invalid number of required columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                    label3.Visible = true;
                    return;
                }

                foreach (var columnName in lines[0].Split(','))
                {
                    if (!accceptedColumns.Contains(columnName))
                    {
                        //Invalid column
                        MessageBox.Show("Selected CSV file doesn't contain column (" + columnName + ").", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text = "";
                        label3.Visible = true;
                        return;
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create a new FolderBrowserDialog
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // Set the initial directory to the user's My Pictures folder
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyPictures;

            // Show the dialog and wait for the user to select a folder
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // Check if the selected folder contains any image files
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                string[] imageFiles = Directory.GetFiles(folderBrowserDialog.SelectedPath)
                                                .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                                .ToArray();

                // If no image files are found, display an error message and ask the user to try again
                if (imageFiles.Length == 0)
                {
                    MessageBox.Show("No images found in this folder. Please select a different folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = string.Empty;
                    label4.Visible = true;
                    label4.Text = "Kindly select a valid folder";
                    label4.ForeColor = Color.Red;
                }
                else
                {
                    // Do something with the selected folder (e.g. pass it to another function)
                    //Console.WriteLine($"Selected folder: {folderBrowserDialog.SelectedPath}");
                    textBox2.Text = folderBrowserDialog.SelectedPath;
                    label4.Visible = false;
                    label4.ForeColor = Color.MediumBlue;
                }
            }
        }

        public void ExtractData(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Get the total number of rows in the CSV file
                int totalRows = File.ReadLines(filePath).Count();

                int totalRec = totalRows - 1;
                // Skip the header row
                reader.ReadLine();
                for (int i = 1; i <= totalRows - 1; i++)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    // Extract specific values for each row
                    //string dogName = values[0];
                    //string gender = values[1];
                    //string breed = values[2];
                    //string color = values[3];
                    //string owner = values[4];
                    //string pabrNo = values[5];
                    //string imageName = values[6];

                    string dogName = values[0];
                    string gender = values[1];
                    string breed = values[2];
                    string color = values[3];
                    string owner = values[4];
                    string pabrNo = values[5];
                    string dob = values[6];
                    string imageName = (!isImageFolderNotAvailable) ? values[7] : "";

                    string fp = (!isImageFolderNotAvailable) ? textBox2.Text + "/" + imageName : "";

                    //Saving
                    AddDog(dogName.Trim(), gender.Trim(), breed.Trim(), color.Trim(), owner.Trim(), pabrNo.Trim(), dob.Trim(), fp.Trim());

                    // Update the progress bar
                    progressBar1.Value = (int)(((double)i / (double)totalRec) * 100);
                    label5.Visible = true;
                    label5.Text = progressBar1.Value.ToString() + " %";

                    // Update the label with the current progress percentage
                    //label.Visible = true;
                    //label.Text = $"{progressBar.Value}%";
                }
            }

            label6.Text = "Status: Upload Completed!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Specify file path", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    isImageFolderNotAvailable = true;
                }

                ExtractData(textBox1.Text.Trim());
                MessageBox.Show("Batch upload successfully completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }

        }

        private bool AddDog(string dogName, string gender, string breed, string color, string owner, string pabrNo, string dob, string picURL)
        {
            bool res = false, uploadSuccess = false;
            string fileName = string.Empty;

            gender = (gender.ToUpper().StartsWith("M")) ? "M" :
                (gender.ToUpper().StartsWith("F")) ? "F" : "";

            if (string.IsNullOrWhiteSpace(pabrNo))
            {
                res = false;
                return res;
            }


            if (!string.IsNullOrWhiteSpace(dob))
            {
                try
                {
                    DateTime dateValue = DateTime.Parse(dob);
                    dob = dateValue.ToString("MM-dd-yyyy");
                }
                catch
                {
                    dob = "";
                }
            }

            fileName = picURL.Trim();

            if (!string.IsNullOrWhiteSpace(fileName.Trim()))
            {
                //Upload Dog Picture to get filename
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://pabrdexapi.com");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);

                    byte[] fileBytes = File.ReadAllBytes(picURL);
                    MemoryStream memoryStream = new MemoryStream(fileBytes);


                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] imageData;
                        imageData = memoryStream.ToArray();

                        // Add the image data to the request body as a ByteArrayContent
                        var imageContent = new ByteArrayContent(imageData);

                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(imageContent, "file", Path.GetFileName(picURL));

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
            }
            //Insert to DB
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");

                pointA:
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var dogParams = new
                {
                    dogName = dogName,
                    //gender = textBox2.Text.Trim(),
                    gender = gender,
                    breed = breed,
                    color = color,
                    dob = dob,
                    ownerName = owner,
                    pabrNo = pabrNo,
                    picURL = fileName
                };
                var json = JsonConvert.SerializeObject(dogParams);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/PedigreeChart/AddDog", content).Result;

                //check response code
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //call login endpoint
                    try
                    {
                        Form1 f1 = new Form1();
                        bool val = f1.IsLoginSuccess(LoginDetails.PuserEmail, LoginDetails.PuserPW);

                        httpClient.DefaultRequestHeaders.Remove("Authorization");

                        if (val)
                        {
                            goto pointA;
                        }
                        else
                        {
                            return res = false;
                        }
                    }
                    catch (Exception)
                    {
                        return res = false;
                    }
                }

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

            return res;
        }
    }
}

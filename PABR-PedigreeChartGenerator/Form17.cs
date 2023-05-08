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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form17 : Form
    {
        private string[] filePath = { };
        public Form17()
        {
            InitializeComponent();
            LoadDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            button3.Text = "Select File(s)";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileNames;

                string[] fileNames = openFileDialog1.SafeFileNames;
                textBox1.Text = string.Join(", ", fileNames);
            }

            button3.Text = "Reselect File(s)";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()))
            {
                textBox1.Focus();
                MessageBox.Show("Select at least one (1) image.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (string file in filePath)
            {
                UploadImage(file);
            }

            MessageBox.Show("Photo(s) Uploaded.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataGridView();
        }


        private bool UploadImage(string filePath)
        {
            bool res = false, uploadSuccess = false;
            string fileName = string.Empty;
            bool isNullOrEmpty = string.IsNullOrWhiteSpace(filePath);

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
                        byte[] imageData = File.ReadAllBytes(filePath);

                        // Add the image data to the request body as a ByteArrayContent
                        var imageContent = new ByteArrayContent(imageData);

                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(imageContent, "file", Path.GetFileName(filePath));

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
            {;
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var dataParams = new
                {
                    image = fileName
                };
                var json = JsonConvert.SerializeObject(dataParams);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/ContentData/InsertEventPhotos", content).Result;

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

        void LoadDataGridView()
        {
            DataTable gallery = new DataTable();
            BindingSource sbGallery= new BindingSource();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=EventPhotos").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        gallery.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = gallery.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        gallery.Rows.Add(row);
                    }

                    sbGallery.DataSource = gallery;

                    dataGridView1.Columns.Clear();

                    // Add checkbox column to DataGridView
                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.HeaderText = "Select";
                    chk.Name = "chk";
                    chk.TrueValue = true;
                    chk.FalseValue = false;
                    dataGridView1.Columns.Insert(0, chk);

                    dataGridView1.DataSource = sbGallery;

                    dataGridView1.Columns[1].HeaderText = "ID";
                    dataGridView1.Columns[2].HeaderText = "File Name";
                    dataGridView1.Columns[3].HeaderText = "Date Added";

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "Action";
                    deleteButton.Name = "Delete";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(deleteButton);

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    dataGridView1.Columns.Clear();
                    return;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Delete this image?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool res = DeleteRecord(dataGridView1.Rows[e.RowIndex].Cells["recID"].Value.ToString());

                    if (res)
                    {
                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete image.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadDataGridView();
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if any checkboxes are selected
            //bool anySelected = false;
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    DataGridViewCheckBoxCell chk = row.Cells["chk"] as DataGridViewCheckBoxCell;
            //    if (chk.Value == chk.TrueValue)
            //    {
            //        anySelected = true;
            //        break;
            //    }
            //}

            //// Set visibility of button based on selection
            //button4.Visible = anySelected;
        }

        private bool DeleteRecord(string ID)
        {
            bool res = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=EventPhotos&ID=" + ID).Result;
                var resp = response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error" && title == "Data Not Deleted")
                {
                    res = false;
                }
                else if (status == "success" && title == "Data Deleted")
                {
                    res = true;
                }
            }

            return res;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> selectedIDs = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells["chk"] as DataGridViewCheckBoxCell;
                if (chk.Value == chk.TrueValue)
                {
                    string id = row.Cells["recID"].Value.ToString();
                    selectedIDs.Add(id);
                }
            }

            if (selectedIDs.Count == 0)
            {
                MessageBox.Show("No selected records", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Delete selected image(s)?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (string value in selectedIDs)
                    {
                        DeleteRecord(value);
                    }
                    MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDataGridView();
                }
            }
        }
    }
}

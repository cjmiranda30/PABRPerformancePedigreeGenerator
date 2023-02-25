using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            loadDogDetails();



        }

        private async void loadDogDetails()
        {
            label1.Text = label1.Text + " " + CurSelectedDog.DogName.Trim();
            label2.Text = label2.Text + " " + CurSelectedDog.Gender.Trim();
            label3.Text = label3.Text + " " + CurSelectedDog.Breed.Trim();
            label6.Text = label6.Text + " " + CurSelectedDog.Color.Trim();
            label5.Text = label5.Text + " " + CurSelectedDog.OwnerName.Trim();
            label4.Text = label4.Text + " " + CurSelectedDog.PABRno.Trim();
            label9.Text = label9.Text + " " + CurSelectedDog.RegistryNo.Trim();
            label8.Text = label8.Text + " " + CurSelectedDog.DateAdded.Trim();


            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = await httpClient.PostAsync("api/PedigreeChart/download-dog-picture?fileName=" + CurSelectedDog.PicURL, null);


                // Get the FileStream from the response content
                var stream = await response.Content.ReadAsStreamAsync();

                // Load the image from the stream into a Bitmap object
                var image = new Bitmap(stream);

                // Display the image in the PictureBox control
                pictureBox1.Image = image;
            }
            label7.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete dog details?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
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
                            (System.Windows.Forms.Application.OpenForms["Form3"] as Form3).LoadDataGridView();
                        }
                        this.Hide();
                    }

                }
            }
        }
    }
}

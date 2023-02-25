using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form7 : Form
    {
        public int curid = 0;
        public string category = "";

        private int FinalDogID = 0;
        public Form7(int curDogID, string category)
        {
            InitializeComponent();

            if (curDogID > 0)
            {
                curid = curDogID;
            }

            this.category = category;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            LoadListBox();


            if (curid > 0)
            {
                listBox1.SelectedValue = curid;

                button2.Enabled = true;
            }
            else
            {
                label1.Text = "Dog Name: N/A";
                label2.Text = "Gender: N/A";
                label3.Text = "Breed: N/A";
                label6.Text = "Color: N/A";
                label5.Text = "Owner: N/A";
                label4.Text = "PABR No.: N/A";
                label9.Text = "Registry No.: N/A";

                pictureBox1.Image = null;
                label7.Text = "";

                button2.Enabled = false;
            }
        }


        public void LoadListBox()
        {
            DataTable dtDog = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetAll", null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                //col
                foreach (var item in jsonList[0])
                {
                    dtDog.Columns.Add(new DataColumn(item.Name, typeof(string)));
                }

                //row
                foreach (var item in jsonList)
                {
                    DataRow row = dtDog.NewRow();
                    foreach (var property in item)
                    {
                        row[property.Name] = property.Value.ToString();
                    }
                    dtDog.Rows.Add(row);

                    if (dtDog.Rows.Count > 0)
                    {
                        if (category.Contains("Sire"))
                        {
                            groupBox1.Text = "Sire List/s:";

                            // Delete rows where Gender value is "F"
                            for (int i = dtDog.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dtDog.Rows[i]["Gender"].ToString() == "F")
                                {
                                    dtDog.Rows.RemoveAt(i);
                                }
                            }
                        }
                        else if (category.Contains("Dam"))
                        {
                            groupBox1.Text = "Dam List/s:";

                            // Delete rows where Gender value is "M"
                            for (int i = dtDog.Rows.Count - 1; i >= 0; i--)
                            {
                                if (dtDog.Rows[i]["Gender"].ToString() == "M")
                                {
                                    dtDog.Rows.RemoveAt(i);
                                }
                            }
                        }

                        // Delete rows where Gender value is "F"
                        for (int i = dtDog.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dtDog.Rows[i]["RecID"].ToString() == CurSelectedDog.UID)
                            {
                                dtDog.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            listBox1.DataSource = dtDog;
            listBox1.DisplayMember = "dogName";
            listBox1.ValueMember = "recID";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Get the search text from the TextBox
            string searchText = textBox1.Text;

            // Search for the first item that starts with the search text
            int index = listBox1.FindString(searchText);

            // If an item is found, select it
            if (index >= 0)
            {
                listBox1.SelectedIndex = index;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedValue = listBox1.SelectedValue;

            try
            {
                // Convert the selected value to an integer
                int selectedIntValue = Convert.ToInt32(selectedValue);

                label1.Text = "Dog Name:";
                label2.Text = "Gender:";
                label3.Text = "Breed:";
                label6.Text = "Color:";
                label5.Text = "Owner:";
                label4.Text = "PABR No.:";
                label9.Text = "Registry No.:";

                pictureBox1.Image = null;
                label7.Text = "Loading.. Please wait:";

                loadDogDetails(selectedIntValue.ToString());
                button2.Enabled = true;

            }
            catch (Exception ex) { }
        }

        public async void loadDogDetails(string id)
        {

            #region DogDetails
            DataTable dtDog = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + id, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDog.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDog.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDog.Rows.Add(row);
                    }
                }
            }

            //populate fields
            if (dtDog.Rows.Count > 0)
            {
                FinalDogID = int.Parse(dtDog.Rows[0][0].ToString());
                label1.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][1].ToString()) ? label1.Text : label1.Text + "   " + dtDog.Rows[0][1].ToString();
                label2.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][2].ToString()) ? label2.Text : label2.Text + "   " + dtDog.Rows[0][2].ToString();
                label3.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][3].ToString()) ? label3.Text : label3.Text + "   " + dtDog.Rows[0][3].ToString();
                label6.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][4].ToString()) ? label6.Text : label6.Text + "   " + dtDog.Rows[0][4].ToString();
                label5.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][5].ToString()) ? label5.Text : label5.Text + "   " + dtDog.Rows[0][5].ToString();
                label4.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][6].ToString()) ? label4.Text : label4.Text + "   " + dtDog.Rows[0][6].ToString();
                label9.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][7].ToString()) ? label9.Text : label9.Text + "   " + dtDog.Rows[0][7].ToString();

                string picurl = string.IsNullOrWhiteSpace(dtDog.Rows[0][8].ToString()) ? "" : dtDog.Rows[0][8].ToString();

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = await httpClient.PostAsync("api/PedigreeChart/download-dog-picture?fileName=" + picurl, null);


                    // Get the FileStream from the response content
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Load the image from the stream into a Bitmap object
                    var image = new Bitmap(stream);

                    // Display the image in the PictureBox control
                    pictureBox1.Image = image;
                }
                label7.Text = string.Empty;
            }
            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (category == "ParentSire")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uParentSire = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeParentSire = true;
                }
                this.Dispose();
            }
            else if (category == "ParentDam")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uParentDam = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeParentDam = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentSire1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentSire1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentSire1 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentDam1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentDam1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentDam1 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentSire2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentSire2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentSire2 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentDam2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentDam2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentDam2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire1 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam1 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire3")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire3 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire3 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam3")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam3 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam3 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire4")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire4 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire4 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam4")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam4 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam4 = true;
                }
                this.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalDogID = 0;
            if (category == "ParentSire")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uParentSire = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeParentSire = true;
                }
                this.Dispose();
            }
            else if (category == "ParentDam")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uParentDam = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeParentDam = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentSire1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentSire1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentSire1 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentDam1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentDam1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentDam1 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentSire2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentSire2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentSire2 = true;
                }
                this.Dispose();
            }
            else if (category == "GrandParentDam2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGrandParentDam2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGrandParentDam2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire1 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam1")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam1 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam1 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam2")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam2 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam2 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire3")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire3 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire3 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam3")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam3 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam3 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentSire4")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentSire4 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentSire4 = true;
                }
                this.Dispose();
            }
            else if (category == "GGrandParentDam4")
            {
                if (System.Windows.Forms.Application.OpenForms["Form2"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).uGGrandParentDam4 = FinalDogID;
                    (System.Windows.Forms.Application.OpenForms["Form2"] as Form2).isChangeGGrandParentDam4 = true;
                }
                this.Dispose();
            }
        }
    }
}

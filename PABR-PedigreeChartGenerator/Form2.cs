using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form2 : Form
    {
        private int ParentSire = 0, ParentDam = 0,

            GrandParentSire1 = 0, GrandParentDam1 = 0, GrandParentSire2 = 0, GrandParentDam2 = 0,

            GGrandParentSire1 = 0, GGrandParentDam1 = 0, GGrandParentSire2 = 0, GGrandParentDam2 = 0,
            GGrandParentSire3 = 0, GGrandParentDam3 = 0, GGrandParentSire4 = 0, GGrandParentDam4 = 0;


        public int uParentSire = 0, uParentDam = 0,

            uGrandParentSire1 = 0, uGrandParentDam1 = 0, uGrandParentSire2 = 0, uGrandParentDam2 = 0,

            uGGrandParentSire1 = 0, uGGrandParentDam1 = 0, uGGrandParentSire2 = 0, uGGrandParentDam2 = 0,
            uGGrandParentSire3 = 0, uGGrandParentDam3 = 0, uGGrandParentSire4 = 0, uGGrandParentDam4 = 0;

        public bool isChangeParentSire = false, isChangeParentDam = false,

            isChangeGrandParentSire1 = false, isChangeGrandParentDam1 = false, isChangeGrandParentSire2 = false, isChangeGrandParentDam2 = false,

            isChangeGGrandParentSire1 = false, isChangeGGrandParentDam1 = false, isChangeGGrandParentSire2 = false, isChangeGGrandParentDam2 = false,
            isChangeGGrandParentSire3 = false, isChangeGGrandParentDam3 = false, isChangeGGrandParentSire4 = false, isChangeGGrandParentDam4 = false;

        public Form2()
        {
            InitializeComponent();

            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;
            label3.Text = label3.Text + "   " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

            ParentSire = 0; ParentDam = 0;
            GrandParentSire1 = 0; GrandParentDam1 = 0; GrandParentSire2 = 0; GrandParentDam2 = 0;
            GGrandParentSire1 = 0; GGrandParentDam1 = 0; GGrandParentSire2 = 0; GGrandParentDam2 = 0;
            GGrandParentSire3 = 0; GGrandParentDam3 = 0; GGrandParentSire4 = 0; GGrandParentDam4 = 0;

            uParentSire = 0; uParentDam = 0;
            uGrandParentSire1 = 0; uGrandParentDam1 = 0; uGrandParentSire2 = 0; uGrandParentDam2 = 0;
            uGGrandParentSire1 = 0; uGGrandParentDam1 = 0; uGGrandParentSire2 = 0; uGGrandParentDam2 = 0;
            uGGrandParentSire3 = 0; uGGrandParentDam3 = 0; uGGrandParentSire4 = 0; uGGrandParentDam4 = 0;

            isChangeParentSire = false; isChangeParentDam = false;
            isChangeGrandParentSire1 = false; isChangeGrandParentDam1 = false; isChangeGrandParentSire2 = false; isChangeGrandParentDam2 = false;
            isChangeGGrandParentSire1 = false; isChangeGGrandParentDam1 = false; isChangeGGrandParentSire2 = false; isChangeGGrandParentDam2 = false;
            isChangeGGrandParentSire3 = false; isChangeGGrandParentDam3 = false; isChangeGGrandParentSire4 = false; isChangeGGrandParentDam4 = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoadParents()
        {
            #region DogDetails
            DataTable dtDog = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + CurSelectedDog.UID, null).Result;
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
                else
                {
                    textBox1.Text = "UNKNOWN";
                    textBox2.Text = "UNKNOWN";
                    textBox3.Text = "UNKNOWN";

                    label7.Text = label7.Text + "   " + "UNKNOWN";
                    label8.Text = label8.Text + "   " + "UNKNOWN";
                    label9.Text = label9.Text + "   " + "UNKNOWN";
                    label10.Text = label10.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDog.Rows.Count > 0)
            {
                textBox1.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][1].ToString()) ? "" : dtDog.Rows[0][1].ToString();
                textBox2.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][6].ToString()) ? "" : dtDog.Rows[0][6].ToString();
                textBox3.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][5].ToString()) ? "" : dtDog.Rows[0][5].ToString();

                label7.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][1].ToString()) ? label7.Text : label7.Text + "   " + dtDog.Rows[0][1].ToString();
                label8.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][2].ToString()) ? label8.Text : label8.Text + "   " + dtDog.Rows[0][2].ToString();
                label9.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][6].ToString()) ? label9.Text : label9.Text + "   " + dtDog.Rows[0][6].ToString();
                label10.Text = string.IsNullOrWhiteSpace(dtDog.Rows[0][4].ToString()) ? label10.Text : label10.Text + "   " + dtDog.Rows[0][4].ToString();

                ParentSire = string.IsNullOrWhiteSpace(dtDog.Rows[0][10].ToString()) ? 0 : int.Parse(dtDog.Rows[0][10].ToString());
                ParentDam = string.IsNullOrWhiteSpace(dtDog.Rows[0][9].ToString()) ? 0 : int.Parse(dtDog.Rows[0][9].ToString());
            }
            #endregion

            #region SireDetails
            DataTable dtDogSire = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + ParentSire, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSire.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSire.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSire.Rows.Add(row);
                    }
                }
                else
                {
                    label14.Text = label14.Text + "   " + "UNKNOWN";
                    label13.Text = label13.Text + "   " + "UNKNOWN";
                    label12.Text = label12.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSire.Rows.Count > 0)
            {
                label14.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][1].ToString()) ? label14.Text : label14.Text + "   " + dtDogSire.Rows[0][1].ToString();
                label13.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][7].ToString()) ? label13.Text : label13.Text + "   " + dtDogSire.Rows[0][7].ToString();
                label12.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][4].ToString()) ? label12.Text : label12.Text + "   " + dtDogSire.Rows[0][4].ToString();

                GrandParentSire1 = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSire.Rows[0][10].ToString());
                GrandParentDam1 = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSire.Rows[0][9].ToString());
            }
            #endregion

            #region DamDetails
            DataTable dtDogDam = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + ParentDam, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDam.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDam.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDam.Rows.Add(row);
                    }
                }
                else
                {
                    label16.Text = label16.Text + "   " + "UNKNOWN";
                    label15.Text = label15.Text + "   " + "UNKNOWN";
                    label11.Text = label11.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDam.Rows.Count > 0)
            {
                label16.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][1].ToString()) ? label16.Text : label16.Text + "   " + dtDogDam.Rows[0][1].ToString();
                label15.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][7].ToString()) ? label15.Text : label15.Text + "   " + dtDogDam.Rows[0][7].ToString();
                label11.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][4].ToString()) ? label11.Text : label11.Text + "   " + dtDogDam.Rows[0][4].ToString();

                GrandParentSire2 = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDam.Rows[0][10].ToString());
                GrandParentDam2 = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDam.Rows[0][9].ToString());
            }
            #endregion
        }
        private void LoadGrandParents()
        {

            #region GrandSireDetails1
            DataTable dtDogSireGP1 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentSire1, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGP1.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGP1.Rows.Add(row);
                    }
                }
                else
                {
                    label19.Text = label19.Text + "   " + "UNKNOWN";
                    label18.Text = label18.Text + "   " + "UNKNOWN";
                    label17.Text = label17.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGP1.Rows.Count > 0)
            {
                label19.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][1].ToString()) ? label19.Text : label19.Text + "   " + dtDogSireGP1.Rows[0][1].ToString();
                label18.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][7].ToString()) ? label18.Text : label18.Text + "   " + dtDogSireGP1.Rows[0][7].ToString();
                label17.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][4].ToString()) ? label17.Text : label17.Text + "   " + dtDogSireGP1.Rows[0][4].ToString();

                GGrandParentSire1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][10].ToString());
                GGrandParentDam1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][9].ToString());
            }
            #endregion

            #region GrandDamDetails1
            DataTable dtDogDamGP1 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentDam1, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGP1.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGP1.Rows.Add(row);
                    }
                }
                else
                {
                    label22.Text = label22.Text + "   " + "UNKNOWN";
                    label21.Text = label21.Text + "   " + "UNKNOWN";
                    label20.Text = label20.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGP1.Rows.Count > 0)
            {
                label22.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][1].ToString()) ? label22.Text : label22.Text + "   " + dtDogDamGP1.Rows[0][1].ToString();
                label21.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][7].ToString()) ? label21.Text : label21.Text + "   " + dtDogDamGP1.Rows[0][7].ToString();
                label20.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][4].ToString()) ? label20.Text : label20.Text + "   " + dtDogDamGP1.Rows[0][4].ToString();

                GGrandParentSire2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][10].ToString());
                GGrandParentDam2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][9].ToString());
            }
            #endregion

            #region GrandSireDetails2
            DataTable dtDogSireGP2 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentSire2, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGP2.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGP2.Rows.Add(row);
                    }
                }
                else
                {
                    label28.Text = label28.Text + "   " + "UNKNOWN";
                    label28.Text = label28.Text + "   " + "UNKNOWN";
                    label26.Text = label26.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGP2.Rows.Count > 0)
            {
                label28.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][1].ToString()) ? label28.Text : label28.Text + "   " + dtDogSireGP2.Rows[0][1].ToString();
                label27.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][7].ToString()) ? label27.Text : label27.Text + "   " + dtDogSireGP2.Rows[0][7].ToString();
                label26.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][4].ToString()) ? label26.Text : label26.Text + "   " + dtDogSireGP2.Rows[0][4].ToString();

                GGrandParentSire3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][10].ToString());
                GGrandParentDam3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][9].ToString());
            }
            #endregion

            #region GrandDamDetails2
            DataTable dtDogDamGP2 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentDam2, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGP2.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGP2.Rows.Add(row);
                    }
                }
                else
                {
                    label25.Text = label25.Text + "   " + "UNKNOWN";
                    label24.Text = label24.Text + "   " + "UNKNOWN";
                    label23.Text = label23.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGP2.Rows.Count > 0)
            {
                label25.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][1].ToString()) ? label25.Text : label25.Text + "   " + dtDogDamGP2.Rows[0][1].ToString();
                label24.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][7].ToString()) ? label24.Text : label24.Text + "   " + dtDogDamGP2.Rows[0][7].ToString();
                label23.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][4].ToString()) ? label23.Text : label23.Text + "   " + dtDogDamGP2.Rows[0][4].ToString();

                GGrandParentSire4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][10].ToString());
                GGrandParentDam4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][9].ToString());
            }
            #endregion
        }
        private void LoadGrandGrandParents()
        {

            #region GGrandSireDetails1
            DataTable dtDogSireGGP1 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire1, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGGP1.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGGP1.Rows.Add(row);
                    }
                }
                else
                {
                    label34.Text = label34.Text + "   " + "UNKNOWN";
                    label33.Text = label33.Text + "   " + "UNKNOWN";
                    label32.Text = label32.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGGP1.Rows.Count > 0)
            {
                label34.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][1].ToString()) ? label34.Text : label34.Text + "   " + dtDogSireGGP1.Rows[0][1].ToString();
                label33.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][7].ToString()) ? label33.Text : label33.Text + "   " + dtDogSireGGP1.Rows[0][7].ToString();
                label32.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][4].ToString()) ? label32.Text : label32.Text + "   " + dtDogSireGGP1.Rows[0][4].ToString();
            }
            #endregion

            #region GGrandDamDetails1
            DataTable dtDogDamGGP1 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam1, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGGP1.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGGP1.Rows.Add(row);
                    }
                }
                else
                {
                    label31.Text = label31.Text + "   " + "UNKNOWN";
                    label30.Text = label30.Text + "   " + "UNKNOWN";
                    label29.Text = label29.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGGP1.Rows.Count > 0)
            {
                label31.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][1].ToString()) ? label31.Text : label31.Text + "   " + dtDogDamGGP1.Rows[0][1].ToString();
                label30.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][7].ToString()) ? label30.Text : label30.Text + "   " + dtDogDamGGP1.Rows[0][7].ToString();
                label29.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][4].ToString()) ? label29.Text : label29.Text + "   " + dtDogDamGGP1.Rows[0][4].ToString();
            }
            #endregion

            #region GGrandSireDetails2
            DataTable dtDogSireGGP2 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire2, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGGP2.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGGP2.Rows.Add(row);
                    }
                }
                else
                {
                    label40.Text = label40.Text + "   " + "UNKNOWN";
                    label39.Text = label39.Text + "   " + "UNKNOWN";
                    label38.Text = label38.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGGP2.Rows.Count > 0)
            {
                label40.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][1].ToString()) ? label40.Text : label40.Text + "   " + dtDogSireGGP2.Rows[0][1].ToString();
                label39.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][7].ToString()) ? label39.Text : label39.Text + "   " + dtDogSireGGP2.Rows[0][7].ToString();
                label38.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][4].ToString()) ? label38.Text : label38.Text + "   " + dtDogSireGGP2.Rows[0][4].ToString();
            }
            #endregion

            #region GGrandDamDetails2
            DataTable dtDogDamGGP2 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam2, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGGP2.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGGP2.Rows.Add(row);
                    }
                }
                else
                {
                    label37.Text = label37.Text + "   " + "UNKNOWN";
                    label36.Text = label36.Text + "   " + "UNKNOWN";
                    label35.Text = label35.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGGP2.Rows.Count > 0)
            {
                label37.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][1].ToString()) ? label37.Text : label37.Text + "   " + dtDogDamGGP2.Rows[0][1].ToString();
                label36.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][7].ToString()) ? label36.Text : label36.Text + "   " + dtDogDamGGP2.Rows[0][7].ToString();
                label35.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][4].ToString()) ? label35.Text : label35.Text + "   " + dtDogDamGGP2.Rows[0][4].ToString();
            }
            #endregion



            #region GGrandSireDetails3
            DataTable dtDogSireGGP3 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire3, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGGP3.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGGP3.Rows.Add(row);
                    }
                }
                else
                {
                    label46.Text = label46.Text + "   " + "UNKNOWN";
                    label45.Text = label45.Text + "   " + "UNKNOWN";
                    label44.Text = label44.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGGP3.Rows.Count > 0)
            {
                label46.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][1].ToString()) ? label46.Text : label46.Text + "   " + dtDogSireGGP3.Rows[0][1].ToString();
                label45.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][7].ToString()) ? label45.Text : label45.Text + "   " + dtDogSireGGP3.Rows[0][7].ToString();
                label44.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][4].ToString()) ? label44.Text : label44.Text + "   " + dtDogSireGGP3.Rows[0][4].ToString();
            }
            #endregion

            #region GrandDamDetails3
            DataTable dtDogDamGGP3 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam3, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGGP3.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGGP3.Rows.Add(row);
                    }
                }
                else
                {
                    label43.Text = label43.Text + "   " + "UNKNOWN";
                    label42.Text = label42.Text + "   " + "UNKNOWN";
                    label41.Text = label41.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGGP3.Rows.Count > 0)
            {
                label43.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][1].ToString()) ? label43.Text : label43.Text + "   " + dtDogDamGGP3.Rows[0][1].ToString();
                label42.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][7].ToString()) ? label42.Text : label42.Text + "   " + dtDogDamGGP3.Rows[0][7].ToString();
                label41.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][4].ToString()) ? label41.Text : label41.Text + "   " + dtDogDamGGP3.Rows[0][4].ToString();
            }
            #endregion

            #region GGrandSireDetails4
            DataTable dtDogSireGGP4 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire4, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogSireGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogSireGGP4.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogSireGGP4.Rows.Add(row);
                    }
                }
                else
                {
                    label52.Text = label52.Text + "   " + "UNKNOWN";
                    label51.Text = label51.Text + "   " + "UNKNOWN";
                    label50.Text = label50.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogSireGGP4.Rows.Count > 0)
            {
                label52.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][1].ToString()) ? label52.Text : label52.Text + "   " + dtDogSireGGP4.Rows[0][1].ToString();
                label51.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][7].ToString()) ? label51.Text : label51.Text + "   " + dtDogSireGGP4.Rows[0][7].ToString();
                label50.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][4].ToString()) ? label50.Text : label50.Text + "   " + dtDogSireGGP4.Rows[0][4].ToString();
            }
            #endregion

            #region GGrandDamDetails4
            DataTable dtDogDamGGP4 = new DataTable();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam4, null).Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        dtDogDamGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = dtDogDamGGP4.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        dtDogDamGGP4.Rows.Add(row);
                    }
                }
                else
                {
                    label49.Text = label49.Text + "   " + "UNKNOWN";
                    label48.Text = label48.Text + "   " + "UNKNOWN";
                    label47.Text = label47.Text + "   " + "UNKNOWN";
                }
            }

            //populate fields
            if (dtDogDamGGP4.Rows.Count > 0)
            {
                label49.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][1].ToString()) ? label49.Text : label49.Text + "   " + dtDogDamGGP4.Rows[0][1].ToString();
                label48.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][7].ToString()) ? label48.Text : label48.Text + "   " + dtDogDamGGP4.Rows[0][7].ToString();
                label47.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][4].ToString()) ? label47.Text : label47.Text + "   " + dtDogDamGGP4.Rows[0][4].ToString();
            }
            #endregion
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadParents();
            LoadGrandParents();
            LoadGrandGrandParents();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(ParentSire, "ParentSire");
            f7.ShowDialog();

            if (isChangeParentSire)
            {
                ParentSire = uParentSire;

                #region SireDetails
                DataTable dtDogSire = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uParentSire, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSire.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSire.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSire.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label14.Text = "Name:   " + "UNKNOWN";
                        label13.Text = "Registry No.:   " + "UNKNOWN";
                        label12.Text = "Color:   " + "UNKNOWN";

                        GrandParentSire1 = 0; GrandParentDam1 = 0;
                        GGrandParentSire1 = 0; GGrandParentDam1 = 0; GGrandParentSire2 = 0; GGrandParentDam2 = 0;
                    }
                }

                //populate fields
                if (dtDogSire.Rows.Count > 0)
                {
                    label14.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][1].ToString()) ? label14.Text : "Name:   " + dtDogSire.Rows[0][1].ToString();
                    label13.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][7].ToString()) ? label13.Text : "Registry No.:   " + dtDogSire.Rows[0][7].ToString();
                    label12.Text = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][4].ToString()) ? label12.Text : "Color:   " + dtDogSire.Rows[0][4].ToString();

                    GrandParentSire1 = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSire.Rows[0][10].ToString());
                    GrandParentDam1 = string.IsNullOrWhiteSpace(dtDogSire.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSire.Rows[0][9].ToString());
                }
                #endregion

                #region GrandSireDetails1
                DataTable dtDogSireGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentSire1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label19.Text = "Name:   " + "UNKNOWN";
                        label18.Text = "Registry No.:   " + "UNKNOWN";
                        label17.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGP1.Rows.Count > 0)
                {
                    label19.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][1].ToString()) ? label19.Text : "Name:   " + dtDogSireGP1.Rows[0][1].ToString();
                    label18.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][7].ToString()) ? label18.Text : "Registry No.:   " + dtDogSireGP1.Rows[0][7].ToString();
                    label17.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][4].ToString()) ? label17.Text : "Color:   " + dtDogSireGP1.Rows[0][4].ToString();

                    GGrandParentSire1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][10].ToString());
                    GGrandParentDam1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][9].ToString());
                }
                #endregion

                #region GrandDamDetails1
                DataTable dtDogDamGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentDam1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label22.Text = "Name:   " + "UNKNOWN";
                        label21.Text = "Registry No.:   " + "UNKNOWN";
                        label20.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGP1.Rows.Count > 0)
                {
                    label22.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][1].ToString()) ? label22.Text : "Name:   " + dtDogDamGP1.Rows[0][1].ToString();
                    label21.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][7].ToString()) ? label21.Text : "Registry No.:   " + dtDogDamGP1.Rows[0][7].ToString();
                    label20.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][4].ToString()) ? label20.Text : "Color:   " + dtDogDamGP1.Rows[0][4].ToString();

                    GGrandParentSire2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][10].ToString());
                    GGrandParentDam2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails1
                DataTable dtDogSireGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label34.Text = "Name:   " + "UNKNOWN";
                        label33.Text = "Registry No.:   " + "UNKNOWN";
                        label32.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP1.Rows.Count > 0)
                {
                    label34.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][1].ToString()) ? label34.Text : "Name:   " + dtDogSireGGP1.Rows[0][1].ToString();
                    label33.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][7].ToString()) ? label33.Text : "Registry No.:   " + dtDogSireGGP1.Rows[0][7].ToString();
                    label32.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][4].ToString()) ? label32.Text : "Color:   " + dtDogSireGGP1.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails1
                DataTable dtDogDamGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label31.Text = "Name:   " + "UNKNOWN";
                        label30.Text = "Registry No.:   " + "UNKNOWN";
                        label29.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP1.Rows.Count > 0)
                {
                    label31.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][1].ToString()) ? label31.Text : "Name:   " + dtDogDamGGP1.Rows[0][1].ToString();
                    label30.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][7].ToString()) ? label30.Text : "Registry No.:   " + dtDogDamGGP1.Rows[0][7].ToString();
                    label29.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][4].ToString()) ? label29.Text : "Color:   " + dtDogDamGGP1.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandSireDetails2
                DataTable dtDogSireGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label40.Text = "Name:   " + "UNKNOWN";
                        label39.Text = "Registry No.:   " + "UNKNOWN";
                        label38.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP2.Rows.Count > 0)
                {
                    label40.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][1].ToString()) ? label40.Text : "Name:   " + dtDogSireGGP2.Rows[0][1].ToString();
                    label39.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][7].ToString()) ? label39.Text : "Registry No.:   " + dtDogSireGGP2.Rows[0][7].ToString();
                    label38.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][4].ToString()) ? label38.Text : "Color:   " + dtDogSireGGP2.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails2
                DataTable dtDogDamGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label37.Text = "Name:   " + "UNKNOWN";
                        label36.Text = "Registry No.:   " + "UNKNOWN";
                        label35.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP2.Rows.Count > 0)
                {
                    label37.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][1].ToString()) ? label37.Text : "Name:   " + dtDogDamGGP2.Rows[0][1].ToString();
                    label36.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][7].ToString()) ? label36.Text : "Registry No.:   " + dtDogDamGGP2.Rows[0][7].ToString();
                    label35.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][4].ToString()) ? label35.Text : "Color:   " + dtDogDamGGP2.Rows[0][4].ToString();
                }
                #endregion

                uParentSire = 0; isChangeParentSire = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(ParentDam, "ParentDam");
            f7.ShowDialog();

            if (isChangeParentDam)
            {
                ParentDam = uParentDam;

                #region DamDetails
                DataTable dtDogDam = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uParentDam, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDam.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDam.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDam.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label16.Text = "Name:   " + "UNKNOWN";
                        label15.Text = "Registry No.:   " + "UNKNOWN";
                        label11.Text = "Color:   " + "UNKNOWN";

                        GrandParentSire2 = 0; GrandParentDam2 = 0;
                        GGrandParentSire3 = 0; GGrandParentDam3 = 0; GGrandParentSire4 = 0; GGrandParentDam4 = 0;
                    }
                }

                //populate fields
                if (dtDogDam.Rows.Count > 0)
                {
                    label16.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][1].ToString()) ? label16.Text : "Name:   " + dtDogDam.Rows[0][1].ToString();
                    label15.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][7].ToString()) ? label15.Text : "Registry No.:   " + dtDogDam.Rows[0][7].ToString();
                    label11.Text = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][4].ToString()) ? label11.Text : "Color:   " + dtDogDam.Rows[0][4].ToString();

                    GrandParentSire2 = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDam.Rows[0][10].ToString());
                    GrandParentDam2 = string.IsNullOrWhiteSpace(dtDogDam.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDam.Rows[0][9].ToString());
                }
                #endregion

                #region GrandSireDetails2
                DataTable dtDogSireGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentSire2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label28.Text = "Name:   " + "UNKNOWN";
                        label28.Text = "Registry No.:   " + "UNKNOWN";
                        label26.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGP2.Rows.Count > 0)
                {
                    label28.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][1].ToString()) ? label28.Text : "Name:   " + dtDogSireGP2.Rows[0][1].ToString();
                    label27.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][7].ToString()) ? label27.Text : "Registry No.:   " + dtDogSireGP2.Rows[0][7].ToString();
                    label26.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][4].ToString()) ? label26.Text : "Color:   " + dtDogSireGP2.Rows[0][4].ToString();

                    GGrandParentSire3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][10].ToString());
                    GGrandParentDam3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][9].ToString());
                }
                #endregion

                #region GrandDamDetails2
                DataTable dtDogDamGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GrandParentDam2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label25.Text = "Name:   " + "UNKNOWN";
                        label24.Text = "Registry No.:   " + "UNKNOWN";
                        label23.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGP2.Rows.Count > 0)
                {
                    label25.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][1].ToString()) ? label25.Text : "Name:   " + dtDogDamGP2.Rows[0][1].ToString();
                    label24.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][7].ToString()) ? label24.Text : "Registry No.:   " + dtDogDamGP2.Rows[0][7].ToString();
                    label23.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][4].ToString()) ? label23.Text : "Color:   " + dtDogDamGP2.Rows[0][4].ToString();

                    GGrandParentSire4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][10].ToString());
                    GGrandParentDam4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails3
                DataTable dtDogSireGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label46.Text = "Name:   " + "UNKNOWN";
                        label45.Text = "Registry No.:   " + "UNKNOWN";
                        label44.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP3.Rows.Count > 0)
                {
                    label46.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][1].ToString()) ? label46.Text : "Name:   " + dtDogSireGGP3.Rows[0][1].ToString();
                    label45.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][7].ToString()) ? label45.Text : "Registry No.:   " + dtDogSireGGP3.Rows[0][7].ToString();
                    label44.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][4].ToString()) ? label44.Text : "Color:   " + dtDogSireGGP3.Rows[0][4].ToString();
                }
                #endregion

                #region GrandDamDetails3
                DataTable dtDogDamGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label43.Text = "Name:   " + "UNKNOWN";
                        label42.Text = "Registry No.:   " + "UNKNOWN";
                        label41.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP3.Rows.Count > 0)
                {
                    label43.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][1].ToString()) ? label43.Text : "Name:   " + dtDogDamGGP3.Rows[0][1].ToString();
                    label42.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][7].ToString()) ? label42.Text : "Registry No.:   " + dtDogDamGGP3.Rows[0][7].ToString();
                    label41.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][4].ToString()) ? label41.Text : "Color:   " + dtDogDamGGP3.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandSireDetails4
                DataTable dtDogSireGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label52.Text = "Name:   " + "UNKNOWN";
                        label51.Text = "Registry No.:   " + "UNKNOWN";
                        label50.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP4.Rows.Count > 0)
                {
                    label52.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][1].ToString()) ? label52.Text : "Name:   " + dtDogSireGGP4.Rows[0][1].ToString();
                    label51.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][7].ToString()) ? label51.Text : "Registry No.:   " + dtDogSireGGP4.Rows[0][7].ToString();
                    label50.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][4].ToString()) ? label50.Text : "Color:   " + dtDogSireGGP4.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails4
                DataTable dtDogDamGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label49.Text = "Name:   " + "UNKNOWN";
                        label48.Text = "Registry No.:   " + "UNKNOWN";
                        label47.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP4.Rows.Count > 0)
                {
                    label49.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][1].ToString()) ? label49.Text : "Name:   " + dtDogDamGGP4.Rows[0][1].ToString();
                    label48.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][7].ToString()) ? label48.Text : "Registry No.:   " + dtDogDamGGP4.Rows[0][7].ToString();
                    label47.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][4].ToString()) ? label47.Text : "Color:   " + dtDogDamGGP4.Rows[0][4].ToString();
                }
                #endregion

                uParentDam = 0; isChangeParentDam = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GrandParentSire1, "GrandParentSire1");
            f7.ShowDialog();

            if (isChangeGrandParentSire1)
            {
                GrandParentSire1 = uGrandParentSire1;

                #region GrandSireDetails1
                DataTable dtDogSireGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGrandParentSire1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label19.Text = "Name:   " + "UNKNOWN";
                        label18.Text = "Registry No.:   " + "UNKNOWN";
                        label17.Text = "Color:   " + "UNKNOWN";

                        GGrandParentSire1 = 0; GGrandParentDam1 = 0;
                    }
                }

                //populate fields
                if (dtDogSireGP1.Rows.Count > 0)
                {
                    label19.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][1].ToString()) ? label19.Text : "Name:   " + dtDogSireGP1.Rows[0][1].ToString();
                    label18.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][7].ToString()) ? label18.Text : "Registry No.:   " + dtDogSireGP1.Rows[0][7].ToString();
                    label17.Text = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][4].ToString()) ? label17.Text : "Color:   " + dtDogSireGP1.Rows[0][4].ToString();

                    GGrandParentSire1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][10].ToString());
                    GGrandParentDam1 = string.IsNullOrWhiteSpace(dtDogSireGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP1.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails1
                DataTable dtDogSireGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label34.Text = "Name:   " + "UNKNOWN";
                        label33.Text = "Registry No.:   " + "UNKNOWN";
                        label32.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP1.Rows.Count > 0)
                {
                    label34.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][1].ToString()) ? label34.Text : "Name:   " + dtDogSireGGP1.Rows[0][1].ToString();
                    label33.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][7].ToString()) ? label33.Text : "Registry No.:   " + dtDogSireGGP1.Rows[0][7].ToString();
                    label32.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][4].ToString()) ? label32.Text : "Color:   " + dtDogSireGGP1.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails1
                DataTable dtDogDamGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label31.Text = "Name:   " + "UNKNOWN";
                        label30.Text = "Registry No.:   " + "UNKNOWN";
                        label29.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP1.Rows.Count > 0)
                {
                    label31.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][1].ToString()) ? label31.Text : "Name:   " + dtDogDamGGP1.Rows[0][1].ToString();
                    label30.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][7].ToString()) ? label30.Text : "Registry No.:   " + dtDogDamGGP1.Rows[0][7].ToString();
                    label29.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][4].ToString()) ? label29.Text : "Color:   " + dtDogDamGGP1.Rows[0][4].ToString();
                }
                #endregion

                uGrandParentSire1 = 0; isChangeGrandParentSire1 = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GrandParentDam1, "GrandParentDam1");
            f7.ShowDialog();

            if (isChangeGrandParentDam1)
            {
                GrandParentDam1 = uGrandParentDam1;

                #region GrandDamDetails1
                DataTable dtDogDamGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGrandParentDam1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label22.Text = "Name:   " + "UNKNOWN";
                        label21.Text = "Registry No.:   " + "UNKNOWN";
                        label20.Text = "Color:   " + "UNKNOWN";

                        GGrandParentSire2 = 0; GGrandParentDam2 = 0;
                    }
                }

                //populate fields
                if (dtDogDamGP1.Rows.Count > 0)
                {
                    label22.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][1].ToString()) ? label22.Text : "Name:   " + dtDogDamGP1.Rows[0][1].ToString();
                    label21.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][7].ToString()) ? label21.Text : "Registry No.:   " + dtDogDamGP1.Rows[0][7].ToString();
                    label20.Text = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][4].ToString()) ? label20.Text : "Color:   " + dtDogDamGP1.Rows[0][4].ToString();

                    GGrandParentSire2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][10].ToString());
                    GGrandParentDam2 = string.IsNullOrWhiteSpace(dtDogDamGP1.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP1.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails2
                DataTable dtDogSireGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label40.Text = "Name:   " + "UNKNOWN";
                        label39.Text = "Registry No.:   " + "UNKNOWN";
                        label38.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP2.Rows.Count > 0)
                {
                    label40.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][1].ToString()) ? label40.Text : "Name:   " + dtDogSireGGP2.Rows[0][1].ToString();
                    label39.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][7].ToString()) ? label39.Text : "Registry No.:   " + dtDogSireGGP2.Rows[0][7].ToString();
                    label38.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][4].ToString()) ? label38.Text : "Color:   " + dtDogSireGGP2.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails2
                DataTable dtDogDamGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label37.Text = "Name:   " + "UNKNOWN";
                        label36.Text = "Registry No.:   " + "UNKNOWN";
                        label35.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP2.Rows.Count > 0)
                {
                    label37.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][1].ToString()) ? label37.Text : "Name:   " + dtDogDamGGP2.Rows[0][1].ToString();
                    label36.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][7].ToString()) ? label36.Text : "Registry No.:   " + dtDogDamGGP2.Rows[0][7].ToString();
                    label35.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][4].ToString()) ? label35.Text : "Color:   " + dtDogDamGGP2.Rows[0][4].ToString();
                }
                #endregion

                uGrandParentDam1 = 0; isChangeGrandParentDam1 = false;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GrandParentSire2, "GrandParentSire2");
            f7.ShowDialog();

            if (isChangeGrandParentSire2)
            {
                GrandParentSire2 = uGrandParentSire2;

                #region GrandSireDetails2
                DataTable dtDogSireGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGrandParentSire2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label28.Text = "Name:   " + "UNKNOWN";
                        label28.Text = "Registry No.:   " + "UNKNOWN";
                        label26.Text = "Color:   " + "UNKNOWN";

                        GGrandParentSire3 = 0; GGrandParentDam3 = 0;
                    }
                }

                //populate fields
                if (dtDogSireGP2.Rows.Count > 0)
                {
                    label28.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][1].ToString()) ? label28.Text : "Name:   " + dtDogSireGP2.Rows[0][1].ToString();
                    label27.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][7].ToString()) ? label27.Text : "Registry No.:   " + dtDogSireGP2.Rows[0][7].ToString();
                    label26.Text = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][4].ToString()) ? label26.Text : "Color:   " + dtDogSireGP2.Rows[0][4].ToString();

                    GGrandParentSire3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][10].ToString());
                    GGrandParentDam3 = string.IsNullOrWhiteSpace(dtDogSireGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogSireGP2.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails3
                DataTable dtDogSireGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label46.Text = "Name:   " + "UNKNOWN";
                        label45.Text = "Registry No.:   " + "UNKNOWN";
                        label44.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP3.Rows.Count > 0)
                {
                    label46.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][1].ToString()) ? label46.Text : "Name:   " + dtDogSireGGP3.Rows[0][1].ToString();
                    label45.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][7].ToString()) ? label45.Text : "Registry No.:   " + dtDogSireGGP3.Rows[0][7].ToString();
                    label44.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][4].ToString()) ? label44.Text : "Color:   " + dtDogSireGGP3.Rows[0][4].ToString();
                }
                #endregion

                #region GrandDamDetails3
                DataTable dtDogDamGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label43.Text = "Name:   " + "UNKNOWN";
                        label42.Text = "Registry No.:   " + "UNKNOWN";
                        label41.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP3.Rows.Count > 0)
                {
                    label43.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][1].ToString()) ? label43.Text : "Name:   " + dtDogDamGGP3.Rows[0][1].ToString();
                    label42.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][7].ToString()) ? label42.Text : "Registry No.:   " + dtDogDamGGP3.Rows[0][7].ToString();
                    label41.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][4].ToString()) ? label41.Text : "Color:   " + dtDogDamGGP3.Rows[0][4].ToString();
                }
                #endregion

                uGrandParentSire2 = 0; isChangeGrandParentSire2 = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GrandParentDam2, "GrandParentDam2");
            f7.ShowDialog();

            if (isChangeGrandParentDam2)
            {
                GrandParentDam2 = uGrandParentDam2;

                #region GrandDamDetails2
                DataTable dtDogDamGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGrandParentDam2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label25.Text = "Name:   " + "UNKNOWN";
                        label24.Text = "Registry No.:   " + "UNKNOWN";
                        label23.Text = "Color:   " + "UNKNOWN";

                        GGrandParentSire4 = 0; GGrandParentDam4 = 0;
                    }
                }

                //populate fields
                if (dtDogDamGP2.Rows.Count > 0)
                {
                    label25.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][1].ToString()) ? label25.Text : "Name:   " + dtDogDamGP2.Rows[0][1].ToString();
                    label24.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][7].ToString()) ? label24.Text : "Registry No.:   " + dtDogDamGP2.Rows[0][7].ToString();
                    label23.Text = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][4].ToString()) ? label23.Text : "Color:   " + dtDogDamGP2.Rows[0][4].ToString();

                    GGrandParentSire4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][10].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][10].ToString());
                    GGrandParentDam4 = string.IsNullOrWhiteSpace(dtDogDamGP2.Rows[0][9].ToString()) ? 0 : int.Parse(dtDogDamGP2.Rows[0][9].ToString());
                }
                #endregion

                #region GGrandSireDetails4
                DataTable dtDogSireGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label52.Text = "Name:   " + "UNKNOWN";
                        label51.Text = "Registry No.:   " + "UNKNOWN";
                        label50.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP4.Rows.Count > 0)
                {
                    label52.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][1].ToString()) ? label52.Text : "Name:   " + dtDogSireGGP4.Rows[0][1].ToString();
                    label51.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][7].ToString()) ? label51.Text : "Registry No.:   " + dtDogSireGGP4.Rows[0][7].ToString();
                    label50.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][4].ToString()) ? label50.Text : "Color:   " + dtDogSireGGP4.Rows[0][4].ToString();
                }
                #endregion

                #region GGrandDamDetails4
                DataTable dtDogDamGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentDam4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label49.Text = "Name:   " + "UNKNOWN";
                        label48.Text = "Registry No.:   " + "UNKNOWN";
                        label47.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP4.Rows.Count > 0)
                {
                    label49.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][1].ToString()) ? label49.Text : "Name:   " + dtDogDamGGP4.Rows[0][1].ToString();
                    label48.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][7].ToString()) ? label48.Text : "Registry No.:   " + dtDogDamGGP4.Rows[0][7].ToString();
                    label47.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][4].ToString()) ? label47.Text : "Color:   " + dtDogDamGGP4.Rows[0][4].ToString();
                }
                #endregion

                uGrandParentDam2 = 0; isChangeGrandParentDam2 = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentSire1, "GGrandParentSire1");
            f7.ShowDialog();

            if (isChangeGGrandParentSire1)
            {
                GGrandParentSire1 = uGGrandParentSire1;

                #region GGrandSireDetails1
                DataTable dtDogSireGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + GGrandParentSire1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label34.Text = "Name:   " + "UNKNOWN";
                        label33.Text = "Registry No.:   " + "UNKNOWN";
                        label32.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP1.Rows.Count > 0)
                {
                    label34.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][1].ToString()) ? label34.Text : "Name:   " + dtDogSireGGP1.Rows[0][1].ToString();
                    label33.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][7].ToString()) ? label33.Text : "Registry No.:   " + dtDogSireGGP1.Rows[0][7].ToString();
                    label32.Text = string.IsNullOrWhiteSpace(dtDogSireGGP1.Rows[0][4].ToString()) ? label32.Text : "Color:   " + dtDogSireGGP1.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentSire1 = 0; isChangeGGrandParentSire1 = false;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentDam1, "GGrandParentDam1");
            f7.ShowDialog();

            if (isChangeGGrandParentDam1)
            {
                GGrandParentDam1 = uGGrandParentDam1;

                #region GGrandDamDetails1
                DataTable dtDogDamGGP1 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentDam1, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP1.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP1.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP1.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label31.Text = "Name:   " + "UNKNOWN";
                        label30.Text = "Registry No.:   " + "UNKNOWN";
                        label29.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP1.Rows.Count > 0)
                {
                    label31.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][1].ToString()) ? label31.Text : "Name:   " + dtDogDamGGP1.Rows[0][1].ToString();
                    label30.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][7].ToString()) ? label30.Text : "Registry No.:   " + dtDogDamGGP1.Rows[0][7].ToString();
                    label29.Text = string.IsNullOrWhiteSpace(dtDogDamGGP1.Rows[0][4].ToString()) ? label29.Text : "Color:   " + dtDogDamGGP1.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentDam1 = 0; isChangeGGrandParentDam1 = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentSire2, "GGrandParentSire2");
            f7.ShowDialog();

            if (isChangeGGrandParentSire2)
            {
                GGrandParentSire2 = uGGrandParentSire2;

                #region GGrandSireDetails2
                DataTable dtDogSireGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentSire2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label40.Text = "Name:   " + "UNKNOWN";
                        label39.Text = "Registry No.:   " + "UNKNOWN";
                        label38.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP2.Rows.Count > 0)
                {
                    label40.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][1].ToString()) ? label40.Text : "Name:   " + dtDogSireGGP2.Rows[0][1].ToString();
                    label39.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][7].ToString()) ? label39.Text : "Registry No.:   " + dtDogSireGGP2.Rows[0][7].ToString();
                    label38.Text = string.IsNullOrWhiteSpace(dtDogSireGGP2.Rows[0][4].ToString()) ? label38.Text : "Color:   " + dtDogSireGGP2.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentSire2 = 0; isChangeGGrandParentSire2 = false;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentDam2, "GGrandParentDam2");
            f7.ShowDialog();

            if (isChangeGGrandParentDam2)
            {
                GGrandParentDam2 = uGGrandParentDam2;

                #region GGrandDamDetails2
                DataTable dtDogDamGGP2 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentDam2, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP2.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP2.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP2.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label37.Text = "Name:   " + "UNKNOWN";
                        label36.Text = "Registry No.:   " + "UNKNOWN";
                        label35.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP2.Rows.Count > 0)
                {
                    label37.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][1].ToString()) ? label37.Text : "Name:   " + dtDogDamGGP2.Rows[0][1].ToString();
                    label36.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][7].ToString()) ? label36.Text : "Registry No.:   " + dtDogDamGGP2.Rows[0][7].ToString();
                    label35.Text = string.IsNullOrWhiteSpace(dtDogDamGGP2.Rows[0][4].ToString()) ? label35.Text : "Color:   " + dtDogDamGGP2.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentDam2 = 0; isChangeGGrandParentDam2 = false;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentSire3, "GGrandParentSire3");
            f7.ShowDialog();

            if (isChangeGGrandParentSire3)
            {
                GGrandParentSire3 = uGGrandParentSire3;

                #region GGrandSireDetails3
                DataTable dtDogSireGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentSire3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label46.Text = "Name:   " + "UNKNOWN";
                        label45.Text = "Registry No.:   " + "UNKNOWN";
                        label44.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP3.Rows.Count > 0)
                {
                    label46.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][1].ToString()) ? label46.Text : "Name:   " + dtDogSireGGP3.Rows[0][1].ToString();
                    label45.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][7].ToString()) ? label45.Text : "Registry No.:   " + dtDogSireGGP3.Rows[0][7].ToString();
                    label44.Text = string.IsNullOrWhiteSpace(dtDogSireGGP3.Rows[0][4].ToString()) ? label44.Text : "Color:   " + dtDogSireGGP3.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentSire3 = 0; isChangeGGrandParentSire3 = false;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentDam3, "GGrandParentDam3");
            f7.ShowDialog();

            if (isChangeGGrandParentDam3)
            {
                GGrandParentDam3 = uGGrandParentDam3;

                #region GrandDamDetails3
                DataTable dtDogDamGGP3 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentDam3, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP3.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP3.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP3.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label43.Text = "Name:   " + "UNKNOWN";
                        label42.Text = "Registry No.:   " + "UNKNOWN";
                        label41.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP3.Rows.Count > 0)
                {
                    label43.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][1].ToString()) ? label43.Text : "Name:   " + dtDogDamGGP3.Rows[0][1].ToString();
                    label42.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][7].ToString()) ? label42.Text : "Registry No.:   " + dtDogDamGGP3.Rows[0][7].ToString();
                    label41.Text = string.IsNullOrWhiteSpace(dtDogDamGGP3.Rows[0][4].ToString()) ? label41.Text : "Color:   " + dtDogDamGGP3.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentDam3 = 0; isChangeGGrandParentDam3 = false;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentSire4, "GGrandParentSire4");
            f7.ShowDialog();

            if (isChangeGGrandParentSire4)
            {
                GGrandParentSire4 = uGGrandParentSire4;

                #region GGrandSireDetails4
                DataTable dtDogSireGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentSire4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogSireGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogSireGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogSireGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label52.Text = "Name:   " + "UNKNOWN";
                        label51.Text = "Registry No.:   " + "UNKNOWN";
                        label50.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogSireGGP4.Rows.Count > 0)
                {
                    label52.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][1].ToString()) ? label52.Text : "Name:   " + dtDogSireGGP4.Rows[0][1].ToString();
                    label51.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][7].ToString()) ? label51.Text : "Registry No.:   " + dtDogSireGGP4.Rows[0][7].ToString();
                    label50.Text = string.IsNullOrWhiteSpace(dtDogSireGGP4.Rows[0][4].ToString()) ? label50.Text : "Color:   " + dtDogSireGGP4.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentSire4 = 0; isChangeGGrandParentSire4 = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(GGrandParentDam4, "GGrandParentDam4");
            f7.ShowDialog();

            if (isChangeGGrandParentDam4)
            {
                GGrandParentDam4 = uGGrandParentDam4;

                #region GGrandDamDetails4
                DataTable dtDogDamGGP4 = new DataTable();

                //get dog details
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                    var response = httpClient.PostAsync("api/PedigreeChart/GetDogDamSire?ID=" + uGGrandParentDam4, null).Result;
                    var resp = response.Content.ReadAsStringAsync();

                    List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                    if (jsonList.Count > 0)
                    {
                        //col
                        foreach (var item in jsonList[0])
                        {
                            dtDogDamGGP4.Columns.Add(new DataColumn(item.Name, typeof(string)));
                        }

                        //row
                        foreach (var item in jsonList)
                        {
                            DataRow row = dtDogDamGGP4.NewRow();
                            foreach (var property in item)
                            {
                                row[property.Name] = property.Value.ToString();
                            }
                            dtDogDamGGP4.Rows.Add(row);
                        }
                    }
                    else
                    {
                        label49.Text = "Name:   " + "UNKNOWN";
                        label48.Text = "Registry No.:   " + "UNKNOWN";
                        label47.Text = "Color:   " + "UNKNOWN";
                    }
                }

                //populate fields
                if (dtDogDamGGP4.Rows.Count > 0)
                {
                    label49.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][1].ToString()) ? label49.Text : "Name:   " + dtDogDamGGP4.Rows[0][1].ToString();
                    label48.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][7].ToString()) ? label48.Text : "Registry No.:   " + dtDogDamGGP4.Rows[0][7].ToString();
                    label47.Text = string.IsNullOrWhiteSpace(dtDogDamGGP4.Rows[0][4].ToString()) ? label47.Text : "Color:   " + dtDogDamGGP4.Rows[0][4].ToString();
                }
                #endregion

                uGGrandParentDam4 = 0; isChangeGGrandParentDam4 = false;
            }
        }

    }
}

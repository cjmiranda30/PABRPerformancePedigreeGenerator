using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) || string.IsNullOrWhiteSpace(textBox2.Text.Trim()))
            {
                MessageBox.Show("Email and Password required.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                bool val = IsLoginSuccess(textBox1.Text.Trim(), textBox2.Text.Trim());

                if (val)
                {
                    Form3 f2 = new Form3();
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    LoginDetails.accessToken = string.Empty;
                    LoginDetails.userID = string.Empty;
                    LoginDetails.userEmail = string.Empty;
                    LoginDetails.userFName = string.Empty;
                    LoginDetails.userLName = string.Empty;

                    MessageBox.Show("Incorrect Email/Password", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #region API Call
        private bool IsLoginSuccess(string un, string pw)
        {
            bool res = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://pabrbullies2022-001-site2.htempurl.com/");
                var lgn = new
                {
                    userEmail = un,
                    passWord = pw
                };
                var json = JsonConvert.SerializeObject(lgn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/login", content).Result;

                var resp = response.Content.ReadAsStringAsync();

                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error" && title == "Invalid Credentials")
                {
                    res = false;
                }
                else if (status == "success" && title == "User Successful Login")
                {
                    res = true;
                    var authToken = responseJson.token;
                    LoginDetails.accessToken = authToken;

                    //get user details
                    var acsTkn = new
                    {
                        token = authToken
                    };

                    var json1 = JsonConvert.SerializeObject(acsTkn);
                    var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);
                    var response1 = httpClient.PostAsync("api/users", content1).Result;
                    var resp1 = response1.Content.ReadAsStringAsync();
                    var responseJson1 = JsonConvert.DeserializeObject<dynamic>(resp1.Result);

                    LoginDetails.userID = responseJson1.uid;
                    LoginDetails.userEmail = responseJson1.userEmail;
                    LoginDetails.userFName = responseJson1.userFirstName;
                    LoginDetails.userLName = responseJson1.userLastName;


                }
            }


            return res;
        }
        #endregion
    }
}

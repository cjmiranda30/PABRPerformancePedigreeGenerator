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
    public partial class Form15 : Form
    {
        public Form15()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.PlaceholderText = "Anonymous";
                textBox1.ReadOnly= true;
            }
            else
            {
                textBox1.PlaceholderText = "";
                textBox1.ReadOnly = false;
                textBox1.Focus();
            }
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

                checkBox1.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text.Trim()) && !checkBox1.Checked)
            {
                textBox1.Focus();
                MessageBox.Show("Customer name is required.\nOtherwise, tick the checkbox to set to Anonymous", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text.Trim()))
            {
                textBox3.Focus();
                MessageBox.Show("Review details is required.", "Sytem Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isInserted = AddTestimonial();

            if (isInserted)
            {
                MessageBox.Show("Review Added.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to add champion.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Dispose();
        }

        private bool AddTestimonial()
        {
            bool res = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var dataParams = new
                {
                    customerName = (checkBox1.Checked) ? "Anonymous" : textBox1.Text.Trim(),
                    review = textBox3.Text.Trim(),
                    dateReviewed = dateTimePicker1.Value.ToString("MM-dd-yyyy")
                };
                var json = JsonConvert.SerializeObject(dataParams);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/ContentData/InsertTestimonial", content).Result;

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
    }
}

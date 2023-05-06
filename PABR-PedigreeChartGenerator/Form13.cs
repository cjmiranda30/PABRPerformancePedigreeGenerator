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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("All fields required.","System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var agentParams = new
                {
                    agentName = textBox1.Text.ToUpper().Trim(),
                    agentContactNo = textBox2.Text.ToUpper().Trim()
                };
                var json = JsonConvert.SerializeObject(agentParams);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("api/Agent/Insert-Agent", content).Result;

                var resp = response.Content.ReadAsStringAsync();

                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error" && title == "Agent Not Registered")
                {
                    MessageBox.Show("Something went wrong.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (status == "success" && title == "Agent Registered")
                {
                    var msg = responseJson.message;
                    MessageBox.Show("Agent registered.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
        }
    }
}

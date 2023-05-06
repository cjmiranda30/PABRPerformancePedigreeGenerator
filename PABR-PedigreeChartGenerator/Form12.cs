using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            // Set timer to tick every second
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;

            LoadAgents();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update label with current time
            label3.Text = "Date and Time:    " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }
        public void LoadAgents()
        {
            DataTable dtAgent = new DataTable();
            BindingSource sbAgent = new BindingSource();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");

                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/Agent/Get-Agents").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count() == 0)
                {
                    dataGridView1.Columns.Clear();
                    return;
                }

                //col
                foreach (var item in jsonList[0])
                {
                    dtAgent.Columns.Add(new DataColumn(item.Name, typeof(string)));
                }

                //row
                foreach (var item in jsonList)
                {
                    DataRow row = dtAgent.NewRow();
                    foreach (var property in item)
                    {
                        row[property.Name] = property.Value.ToString();
                    }
                    dtAgent.Rows.Add(row);
                }
            }
            sbAgent.DataSource = dtAgent;

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = sbAgent;

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Agent Name";
            dataGridView1.Columns[2].HeaderText = "Contact No.";
            dataGridView1.Columns[3].HeaderText = "Active";
            dataGridView1.Columns[4].HeaderText = "Date Added";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Form9 f9 = new Form9();
            f9.ShowDialog();
        }

        private void Search()
        {
            string searchText = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                //((BindingSource)dataGridView1.DataSource).Filter = "[recID] LIKE '%" + searchText + "%' OR [dogName] LIKE '%" + searchText + "%' OR [gender] LIKE '%" + searchText + "%'" +
                //" OR [breed] LIKE '%" + searchText + "%' OR [color] LIKE '%" + searchText + "%'" +
                //" OR [doB] LIKE '%" + searchText + "%' OR [ownerName] LIKE '%" + searchText + "%'" +
                //" OR [pabrNo] LIKE '%" + searchText + "%'";

                ((BindingSource)dataGridView1.DataSource).Filter = "[agentID] LIKE '%" + searchText.Replace("'", "''") + "%' OR [agentName] LIKE '%" + searchText.Replace("'", "''") + "%' OR [agentContactNo] LIKE '%" + searchText.Replace("'", "''") + "%'";
            }
            else
            {
                ((BindingSource)dataGridView1.DataSource).Filter = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                var selectedRow = dataGridView1.Rows[e.RowIndex];

                // Get the values of the cells in the row
                var cellValues = new List<string>();
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    cellValues.Add(cell.Value?.ToString() ?? "");
                }

                // Concatenate the cell values into a single string
                var rowValues = string.Join(",", cellValues);
                string[] values = rowValues.Split(',');

                DialogResult dr = MessageBox.Show("Delete this agent?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                        var response = httpClient.DeleteAsync("api/Agent/Delete-Agent?ID=" + values[0]).Result;
                        var resp = response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                        var status = responseJson.status;
                        var title = responseJson.title;

                        if (status == "error" && title == "Agent Not Deleted")
                        {
                            MessageBox.Show("Unable to delete agent.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (status == "success" && title == "Agent Deleted")
                        {
                            MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAgents();
                        }

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form13 f13 = new Form13();
            f13.ShowDialog();
            LoadAgents();
        }
    }
}

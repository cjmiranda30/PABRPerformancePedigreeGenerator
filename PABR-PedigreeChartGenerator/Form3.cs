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
using System.Windows.Forms.VisualStyles;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                label6.Text = "REGISTERED DOG RECORDS";
                this.Text = "PABR - REGISTERED DOG RECORDS";
            }
            else
            {
                label6.Text = "PERFORMANCE PEDIGREE RECORDS";
                this.Text = "PABR - PERFORMANCE PEDIGREE RECORDS";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Logout now?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                LoginDetails.ClearProperties();
                CurSelectedDog.ClearProperties();

                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;
            label3.Text = label3.Text + "   " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

            LoadDataGridView();
            this.Text = "PABR - REGISTERED DOG RECORDS";
        }

        public void LoadDataGridView()
        {
            DataTable dtDog = new DataTable();
            BindingSource sbDog = new BindingSource();

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
                }
            }
            sbDog.DataSource = dtDog;

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = sbDog;


            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Dog Name";
            dataGridView1.Columns[2].HeaderText = "Gender";
            dataGridView1.Columns[3].HeaderText = "Breed";
            dataGridView1.Columns[4].HeaderText = "Color";
            dataGridView1.Columns[5].HeaderText = "Owner Name";
            dataGridView1.Columns[6].HeaderText = "PABR No.";
            dataGridView1.Columns[7].HeaderText = "Registry No.";
            dataGridView1.Columns[8].HeaderText = "Date Added";
            dataGridView1.Columns[9].HeaderText = "PictureURL";

            dataGridView1.Columns[9].Visible = false; //PictureURL

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Search();
                e.Handled = true;
            }
        }

        private void Search()
        {
            string searchText = textBox1.Text;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                dataGridView1.CurrentCell = null;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool match = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            match = true;
                            break;
                        }
                    }

                    row.Visible = match;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ShowDialog();
            LoadDataGridView();
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

                // Use the rowValues string variable as needed
                CurSelectedDog.UID = values[0];
                CurSelectedDog.DogName = values[1];
                CurSelectedDog.Gender = values[2];
                CurSelectedDog.Breed = values[3];
                CurSelectedDog.Color = values[4];
                CurSelectedDog.OwnerName = values[5];
                CurSelectedDog.PABRno = values[6];
                CurSelectedDog.RegistryNo = values[7];
                CurSelectedDog.DateAdded = values[8];
                CurSelectedDog.PicURL = values[9];

                Form6 f6 = new Form6();
                f6.Show();

                //LoadDataGridView();
            }
        }
    }
}

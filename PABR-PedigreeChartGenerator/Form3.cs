using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static PABR_PedigreeChartGenerator.DogDetails;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form3 : Form
    {
        public static DataTable dogs;
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

            // Set timer to tick every second
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;
            //label3.Text = label3.Text + "   " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

            LoadDataGridView();
            this.Text = "PABR - REGISTERED DOG RECORDS";
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update label with current time
            label3.Text = "Date and Time:    " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }
        public void LoadDataGridView()
        {
            DataTable dtDog = new DataTable();
            BindingSource sbDog = new BindingSource();

            //get dog details
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");

            pointA:
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.PostAsync("api/PedigreeChart/GetAll", null).Result;
                var resp = response.Content.ReadAsStringAsync();

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
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                JArray jsonArray = JArray.Parse(resp.Result);
                dtDog= JsonConvert.DeserializeObject<DataTable>(jsonArray.ToString());


                //List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);


                //if (jsonList.Count() == 0)
                //{
                //    return;
                //}

                ////col
                //foreach (var item in jsonList[0])
                //{
                //    dtDog.Columns.Add(new DataColumn(item.Name, typeof(string)));
                //}

                ////row
                //foreach (var item in jsonList)
                //{
                //    DataRow row = dtDog.NewRow();
                //    foreach (var property in item)
                //    {
                //        row[property.Name] = property.Value.ToString();
                //    }
                //    dtDog.Rows.Add(row);
                //}
            }
            dogs = new DataTable();
            dogs.Clear();
            dogs = dtDog;

            string end = string.Empty;

            #region Commented - causing slowness
            //sbDog.DataSource = dtDog;

            //dataGridView1.Columns.Clear();
            //dataGridView1.DataSource = sbDog;


            //dataGridView1.Columns[0].HeaderText = "ID";
            //dataGridView1.Columns[1].HeaderText = "Dog Name";
            //dataGridView1.Columns[2].HeaderText = "Gender";
            //dataGridView1.Columns[3].HeaderText = "Breed";
            //dataGridView1.Columns[4].HeaderText = "Color";
            //dataGridView1.Columns[5].HeaderText = "Date of Birth";
            //dataGridView1.Columns[6].HeaderText = "Owner Name";
            //dataGridView1.Columns[7].HeaderText = "PABR No.";
            //dataGridView1.Columns[8].HeaderText = "Date Added";
            //dataGridView1.Columns[9].HeaderText = "PictureURL";

            //dataGridView1.Columns[9].Visible = false; //PictureURL

            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            ////dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
            #endregion


            #region Filter to Dam and Sire

            #region DAM

            #region Commented - causing slowness
            //DamSireDataTable.DamData = new DataTable();
            //// Copy the structure of the original DataGridView to the filteredData
            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    DamSireDataTable.DamData.Columns.Add(column.Name);
            //}

            //// Clear the filteredData table to ensure it's empty
            //DamSireDataTable.DamData.Clear();

            //// Loop through each row in the dataGridView1
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    // Get the value of the "Gender" column in the current row
            //    string gender = row.Cells["Gender"].Value.ToString();

            //    // Check if the gender is not "Male"
            //    if (!string.Equals(gender, "M", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // If the gender is not "Male", create a new row in the filteredData
            //        DataRow newRow = DamSireDataTable.DamData.NewRow();

            //        // Copy the values of all columns in the current row to the newRow
            //        foreach (DataGridViewCell cell in row.Cells)
            //        {
            //            newRow[cell.ColumnIndex] = cell.Value;
            //        }

            //        // Add the newRow to the filteredData
            //        DamSireDataTable.DamData.Rows.Add(newRow);
            //    }
            //}
            #endregion

            DamSireDataTable.DamData = new DataTable();

            //// Copy the structure of the original DataTable to the filteredData
            //foreach (DataColumn column in dtDog.Columns)
            //{
            //    DamSireDataTable.DamData.Columns.Add(column.ColumnName, column.DataType);
            //}

            //// Clear the filteredData table to ensure it's empty
            //DamSireDataTable.DamData.Clear();

            //// Loop through each row in the dtDogs DataTable
            //foreach (DataRow row in dtDog.Rows)
            //{
            //    // Get the value of the "Gender" column in the current row
            //    string gender = row["Gender"].ToString();

            //    // Check if the gender is not "Male"
            //    if (!string.Equals(gender, "M", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // If the gender is not "Male", create a new row in the filteredData
            //        DataRow newRow = DamSireDataTable.DamData.NewRow();

            //        // Copy the values of all columns in the current row to the newRow
            //        foreach (DataColumn column in DamSireDataTable.DamData.Columns)
            //        {
            //            newRow[column.ColumnName] = row[column.ColumnName];
            //        }

            //        // Add the newRow to the filteredData
            //        DamSireDataTable.DamData.Rows.Add(newRow);
            //    }
            //}

            // Filter the DataTable by gender using LINQ
            string filterExpressionDam = "Gender = 'F'";
            DataRow[] filteredRowsDam = dtDog.Select(filterExpressionDam);

            // Create a new DataTable and copy the structure of the original DataTable
            DamSireDataTable.DamData = dtDog.Clone();

            // Import the filtered rows into the new DataTable
            DamSireDataTable.DamData = filteredRowsDam.CopyToDataTable();
            #endregion

            #region SIRE

            #region Commented - causing slowness
            //DamSireDataTable.SireData = new DataTable();
            //// Copy the structure of the original DataGridView to the filteredData
            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    DamSireDataTable.SireData.Columns.Add(column.Name);
            //}

            //// Clear the filteredData table to ensure it's empty
            //DamSireDataTable.SireData.Clear();

            //// Loop through each row in the dataGridView1
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    // Get the value of the "Gender" column in the current row
            //    string gender = row.Cells["Gender"].Value.ToString();

            //    // Check if the gender is not "Male"
            //    if (!string.Equals(gender, "F", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // If the gender is not "Male", create a new row in the filteredData
            //        DataRow newRow = DamSireDataTable.SireData.NewRow();

            //        // Copy the values of all columns in the current row to the newRow
            //        foreach (DataGridViewCell cell in row.Cells)
            //        {
            //            newRow[cell.ColumnIndex] = cell.Value;
            //        }

            //        // Add the newRow to the filteredData
            //        DamSireDataTable.SireData.Rows.Add(newRow);
            //    }
            //}
            #endregion

            DamSireDataTable.SireData = new DataTable();

            //// Copy the structure of the original DataTable to the filteredData
            //foreach (DataColumn column in dtDog.Columns)
            //{
            //    DamSireDataTable.SireData.Columns.Add(column.ColumnName, column.DataType);
            //}

            //// Clear the filteredData table to ensure it's empty
            //DamSireDataTable.SireData.Clear();

            //// Loop through each row in the dtDogs DataTable
            //foreach (DataRow row in dtDog.Rows)
            //{
            //    // Get the value of the "Gender" column in the current row
            //    string gender = row["Gender"].ToString();

            //    // Check if the gender is not "Male"
            //    if (!string.Equals(gender, "F", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // If the gender is not "Male", create a new row in the filteredData
            //        DataRow newRow = DamSireDataTable.SireData.NewRow();

            //        // Copy the values of all columns in the current row to the newRow
            //        foreach (DataColumn column in DamSireDataTable.SireData.Columns)
            //        {
            //            newRow[column.ColumnName] = row[column.ColumnName];
            //        }

            //        // Add the newRow to the filteredData
            //        DamSireDataTable.SireData.Rows.Add(newRow);
            //    }
            //}

            // Filter the DataTable by gender using LINQ
            string filterExpressionSire = "Gender = 'M'";
            DataRow[] filteredRowsSire = dtDog.Select(filterExpressionSire);

            // Create a new DataTable and copy the structure of the original DataTable
            DamSireDataTable.SireData = dtDog.Clone();

            // Import the filtered rows into the new DataTable
            DamSireDataTable.SireData = filteredRowsSire.CopyToDataTable();
            #endregion

            #endregion
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

        private void SearchV2()
        {
            string searchText = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                //((BindingSource)dataGridView1.DataSource).Filter = "[recID] LIKE '%" + searchText + "%' OR [dogName] LIKE '%" + searchText + "%' OR [gender] LIKE '%" + searchText + "%'" +
                //" OR [breed] LIKE '%" + searchText + "%' OR [color] LIKE '%" + searchText + "%'" +
                //" OR [doB] LIKE '%" + searchText + "%' OR [ownerName] LIKE '%" + searchText + "%'" +
                //" OR [pabrNo] LIKE '%" + searchText + "%'";

                ((BindingSource)dataGridView1.DataSource).Filter = "[recID] LIKE '%" + searchText.Replace("'", "''") + "%' OR [dogName] LIKE '%" + searchText.Replace("'", "''") + "%' OR [gender] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [breed] LIKE '%" + searchText.Replace("'", "''") + "%' OR [color] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [doB] LIKE '%" + searchText.Replace("'", "''") + "%' OR [ownerName] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [pabrNo] LIKE '%" + searchText.Replace("'", "''") + "%'";
            }
            else
            {
                ((BindingSource)dataGridView1.DataSource).Filter = "";
            }
        }

        public void SearchV3()
        {
            BindingSource sbDog = new BindingSource();
            string searchText = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                //((BindingSource)dataGridView1.DataSource).Filter = "[recID] LIKE '%" + searchText + "%' OR [dogName] LIKE '%" + searchText + "%' OR [gender] LIKE '%" + searchText + "%'" +
                //" OR [breed] LIKE '%" + searchText + "%' OR [color] LIKE '%" + searchText + "%'" +
                //" OR [doB] LIKE '%" + searchText + "%' OR [ownerName] LIKE '%" + searchText + "%'" +
                //" OR [pabrNo] LIKE '%" + searchText + "%'";
                //[recID] LIKE '%" + searchText.Replace("'", "''") + "%' OR 

                dogs.DefaultView.RowFilter = "[dogName] LIKE '%" + searchText.Replace("'", "''") + "%' OR [gender] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [breed] LIKE '%" + searchText.Replace("'", "''") + "%' OR [color] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [doB] LIKE '%" + searchText.Replace("'", "''") + "%' OR [ownerName] LIKE '%" + searchText.Replace("'", "''") + "%'" +
                " OR [pabrNo] LIKE '%" + searchText.Replace("'", "''") + "%'";

                sbDog.DataSource = dogs;

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = sbDog;


                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Dog Name";
                dataGridView1.Columns[2].HeaderText = "Gender";
                dataGridView1.Columns[3].HeaderText = "Breed";
                dataGridView1.Columns[4].HeaderText = "Color";
                dataGridView1.Columns[5].HeaderText = "Date of Birth";
                dataGridView1.Columns[6].HeaderText = "Owner Name";
                dataGridView1.Columns[7].HeaderText = "PABR No.";
                dataGridView1.Columns[8].HeaderText = "Date Added";
                dataGridView1.Columns[9].HeaderText = "PictureURL";

                dataGridView1.Columns[9].Visible = false; //PictureURL

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                dogs.DefaultView.RowFilter = "";
                dataGridView1.Columns.Clear();

            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Search();
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
            //LoadDataGridView();
            if (!string.IsNullOrWhiteSpace(DogDetails.UID))
            {
                AddDogToTable(DogDetails.UID, DogDetails.DogName, DogDetails.Gender, DogDetails.Breed, DogDetails.Color, DogDetails.DoB, DogDetails.OwnerName, DogDetails.PABRno, DogDetails.PicURL);
            }
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
                CurSelectedDog.DoB = values[5];
                CurSelectedDog.OwnerName = values[6];
                CurSelectedDog.PABRno = values[7];
                CurSelectedDog.DateAdded = values[8];
                CurSelectedDog.PicURL = values[9];

                Form6 f6 = new Form6();
                f6.Show();

                //LoadDataGridView();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.ShowDialog();
            //LoadDataGridView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SearchV3();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Form9 f9 = new Form9();
            f9.ShowDialog();
        }

        public static void AddDogToTable(string uid, string dogName, string gender, string breed, string color, string dob, string ownerName, string pabrNo, string picUrl)
        {
            DataRow newRow = dogs.NewRow();
            newRow["recID"] = uid;
            newRow["dogName"] = dogName;
            newRow["gender"] = gender;
            newRow["breed"] = breed;
            newRow["color"] = color;
            newRow["doB"] = dob;
            newRow["ownerName"] = ownerName;
            newRow["pabrNo"] = pabrNo;
            newRow["dateCreated"] = DateTime.Now;
            newRow["picURL"] = picUrl;

            dogs.Rows.Add(newRow);

            if (gender == "M")
            {
                DataRow newDogRow = DamSireDataTable.SireData.NewRow();
                newDogRow["recID"] = uid;
                newDogRow["dogName"] = dogName;
                newDogRow["gender"] = gender;
                newDogRow["breed"] = breed;
                newDogRow["color"] = color;
                newDogRow["doB"] = dob;
                newDogRow["ownerName"] = ownerName;
                newDogRow["pabrNo"] = pabrNo;
                newDogRow["dateCreated"] = DateTime.Now;
                newDogRow["picURL"] = picUrl;

                DamSireDataTable.SireData.Rows.Add(newDogRow);
            }
            else
            {
                DataRow newDogRow = DamSireDataTable.DamData.NewRow();
                newDogRow["recID"] = uid;
                newDogRow["dogName"] = dogName;
                newDogRow["gender"] = gender;
                newDogRow["breed"] = breed;
                newDogRow["color"] = color;
                newDogRow["doB"] = dob;
                newDogRow["ownerName"] = ownerName;
                newDogRow["pabrNo"] = pabrNo;
                newDogRow["dateCreated"] = DateTime.Now;
                newDogRow["picURL"] = picUrl;

                DamSireDataTable.DamData.Rows.Add(newDogRow);
            }


            DogDetails.ClearProperties();
        }

        public static void UpdateDogToTable(string uid, string dogName, string gender, string breed, string color, string dob, string ownerName, string pabrNo, string picUrl)
        {
            int recIdToUpdate = int.Parse(uid);

            DataRow[] rowsToUpdate = dogs.Select($"RecID = '{recIdToUpdate}'");

            if (rowsToUpdate.Length > 0)
            {
                DataRow rowToUpdate = rowsToUpdate[0];
                rowToUpdate["dogName"] = dogName;
                rowToUpdate["gender"] = gender;
                rowToUpdate["breed"] = breed;
                rowToUpdate["color"] = color;
                rowToUpdate["doB"] = dob;
                rowToUpdate["ownerName"] = ownerName;
                rowToUpdate["pabrNo"] = pabrNo;

                if (picUrl == "")
                {
                    rowToUpdate["picURL"] = CurSelectedDog.PicURL;
                }
                else if (picUrl == "Removed")
                {
                    rowToUpdate["picURL"] = "";

                }
                else
                {
                    rowToUpdate["picURL"] = picUrl;

                }
            }

            if (gender != CurSelectedDog.Gender)
            {
                if (gender == "M")
                {
                    //if originally an F, delete then insert to M
                    int recIdToDelete = int.Parse(uid);
                    DataRow[] rowsToDelete = DamSireDataTable.DamData.Select($"RecID = '{recIdToDelete}'");

                    if (rowsToDelete.Length > 0)
                    {
                        DataRow rowToDelete = rowsToDelete[0];
                        rowToDelete.Delete();

                        //add
                        DataRow newDogRow = DamSireDataTable.SireData.NewRow();
                        newDogRow["recID"] = uid;
                        newDogRow["dogName"] = dogName;
                        newDogRow["gender"] = gender;
                        newDogRow["breed"] = breed;
                        newDogRow["color"] = color;
                        newDogRow["doB"] = dob;
                        newDogRow["ownerName"] = ownerName;
                        newDogRow["pabrNo"] = pabrNo;
                        newDogRow["dateCreated"] = DateTime.Now;
                        newDogRow["picURL"] = picUrl;

                        if (picUrl == "")
                        {
                            newDogRow["picURL"] = CurSelectedDog.PicURL;
                        }
                        else if (picUrl == "Removed")
                        {
                            newDogRow["picURL"] = "";

                        }
                        else
                        {
                            newDogRow["picURL"] = picUrl;

                        }

                        DamSireDataTable.SireData.Rows.Add(newDogRow);
                    }
                }
                else if (gender == "F")
                {
                    //if originally an M, delete then insert to F
                    int recIdToDelete = int.Parse(uid);
                    DataRow[] rowsToDelete = DamSireDataTable.SireData.Select($"RecID = '{recIdToDelete}'");

                    if (rowsToDelete.Length > 0)
                    {
                        DataRow rowToDelete = rowsToDelete[0];
                        rowToDelete.Delete();

                        //add
                        DataRow newDogRow = DamSireDataTable.DamData.NewRow();
                        newDogRow["recID"] = uid;
                        newDogRow["dogName"] = dogName;
                        newDogRow["gender"] = gender;
                        newDogRow["breed"] = breed;
                        newDogRow["color"] = color;
                        newDogRow["doB"] = dob;
                        newDogRow["ownerName"] = ownerName;
                        newDogRow["pabrNo"] = pabrNo;
                        newDogRow["dateCreated"] = DateTime.Now;
                        newDogRow["picURL"] = picUrl;

                        if (picUrl == "")
                        {
                            newDogRow["picURL"] = CurSelectedDog.PicURL;
                        }
                        else if (picUrl == "Removed")
                        {
                            newDogRow["picURL"] = "";

                        }
                        else
                        {
                            newDogRow["picURL"] = picUrl;

                        }

                        DamSireDataTable.DamData.Rows.Add(newDogRow);
                    }


                }
            }
        }

        public static void DeleteDogToTable(string uid)
        {
            int recIdToDelete = int.Parse(uid);
            DataRow[] rowsToDeleteDog = dogs.Select($"RecID = '{recIdToDelete}'");
            DataRow[] rowsToDeleteDam = DamSireDataTable.DamData.Select($"RecID = '{recIdToDelete}'");
            DataRow[] rowsToDeleteSire = DamSireDataTable.SireData.Select($"RecID = '{recIdToDelete}'");

            if (rowsToDeleteDog.Length > 0)
            {
                DataRow rowToDelete = rowsToDeleteDog[0];
                rowToDelete.Delete();
            }

            if (rowsToDeleteDam.Length > 0)
            {
                DataRow rowToDelete = rowsToDeleteDam[0];
                rowToDelete.Delete();
            }

            if (rowsToDeleteSire.Length > 0)
            {
                DataRow rowToDelete = rowsToDeleteSire[0];
                rowToDelete.Delete();
            }
        }
    }
}

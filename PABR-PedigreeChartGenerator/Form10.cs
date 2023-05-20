using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABR_PedigreeChartGenerator
{
    public partial class Form10 : Form
    {
        private int currentIndex = 0;
        private bool isPaused = false;
        private string[] webPaths;
        public Form10()
        {
            InitializeComponent();

            tabControl1.SelectedIndex = 0;
            button3.BackColor = Color.Silver;
            LoadEventPoster();

        }

        void LoadEventPoster()
        {
            #region Clear Page
            while (tabPage1.Controls.Count > 0)
            {
                Control control = tabPage1.Controls[0];
                tabPage1.Controls.Remove(control);
                control.Dispose();
            }
            #endregion

            #region Fetch Details
            DataTable eventPoster = new DataTable();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=EventPoster").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        eventPoster.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = eventPoster.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        eventPoster.Rows.Add(row);
                    }
                }

                if (eventPoster.Rows.Count > 0)
                {
                    int x = 10;
                    int y = 30;
                    int pictureBoxWidth = 200;
                    int pictureBoxHeight = 300;
                    int pictureBoxSpacing = 250;
                    int pictureBoxsPerRow = 2;

                    for (int i = 0; i < eventPoster.Rows.Count; i++)
                    {
                        string scheduleddate = eventPoster.Rows[i]["scheduledDate"].ToString();
                        DateTime date = DateTime.ParseExact(scheduleddate, "M-d-yyyy", CultureInfo.InvariantCulture);
                        string formattedDate = date.ToString("dddd, MMMM d, yyyy");

                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Load("https://pabrdex.com/contentimages/" + eventPoster.Rows[i]["image"].ToString());
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.Location = new Point(x, y);
                        pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
                        tabPage1.Controls.Add(pictureBox);

                        // Add label to display the image name on the right side of the picture box
                        Label label = new Label();
                        label.UseMnemonic = false;
                        label.Text = eventPoster.Rows[i]["title"].ToString() +
                            "\n" + formattedDate +
                            "\n" + eventPoster.Rows[i]["location"].ToString() +
                            "\n\n" + eventPoster.Rows[i]["description"].ToString();
                        label.Location = new Point(x + pictureBoxWidth + 10, y);
                        label.AutoSize = true;
                        tabPage1.Controls.Add(label);

                        // Add button below the label
                        Button button = new Button();
                        button.Name = eventPoster.Rows[i]["recID"].ToString();
                        button.Text = "Remove Event";
                        button.BackColor = Color.IndianRed;
                        button.ForeColor = Color.White;
                        button.Location = new Point(x + pictureBoxWidth + 10, y + label.Height + 10);
                        button.Size = new Size(100, 30);
                        button.Click += (s, e) =>
                        {
                            // show message box with image name
                            DialogResult dr = MessageBox.Show("Remove this event?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                                    var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=EventPoster&ID=" + button.Name).Result;
                                    var resp = response.Content.ReadAsStringAsync();
                                    var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                                    var status = responseJson.status;
                                    var title = responseJson.title;

                                    if (status == "error" && title == "Data Not Deleted")
                                    {
                                        MessageBox.Show("Unable to delete event.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else if (status == "success" && title == "Data Deleted")
                                    {
                                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    LoadEventPoster();
                                }
                            }
                        };
                        tabPage1.Controls.Add(button);

                        x += pictureBoxWidth + pictureBoxSpacing;
                        if ((i + 1) % pictureBoxsPerRow == 0)
                        {
                            x = 10;
                            y += pictureBoxHeight + 10; // add button height and spacing
                        }
                    }
                }
            }
            #endregion
        }

        void LoadChampions()
        {
            #region Clear Page
            while (tabPage5.Controls.Count > 0)
            {
                Control control = tabPage5.Controls[0];
                tabPage5.Controls.Remove(control);
                control.Dispose();
            }
            #endregion

            #region Fetch Details
            DataTable champions = new DataTable();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=PABRChampions").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        champions.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = champions.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        champions.Rows.Add(row);
                    }
                }

                if (champions.Rows.Count > 0)
                {
                    int x = 10;
                    int y = 30;
                    int pictureBoxWidth = 200;
                    int pictureBoxHeight = 200;
                    int pictureBoxSpacing = 250;
                    int pictureBoxsPerRow = 2;

                    for (int i = 0; i < champions.Rows.Count; i++)
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Load("https://pabrdex.com/contentimages/" + champions.Rows[i]["dogPicture"].ToString());
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.Location = new Point(x, y);
                        pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
                        tabPage5.Controls.Add(pictureBox);

                        // Add label to display the image name on the right side of the picture box
                        Label label = new Label();
                        label.UseMnemonic = false;
                        label.Text = "Name: " + champions.Rows[i]["dogName"].ToString() +
                            "\nOwner: " + (string.IsNullOrWhiteSpace(champions.Rows[i]["dogOwnerName"].ToString()) ? "N/A" : champions.Rows[i]["dogOwnerName"].ToString()) +
                            "\nCertificate: " + (string.IsNullOrWhiteSpace(champions.Rows[i]["dogCertificate"].ToString()) ? "N/A" : "Available");
                        label.Location = new Point(x + pictureBoxWidth + 10, y);
                        label.AutoSize = true;
                        tabPage5.Controls.Add(label);

                        // Add button below the label
                        Button button = new Button();
                        button.Name = champions.Rows[i]["recID"].ToString();
                        button.Text = "Remove Champion";
                        button.BackColor = Color.IndianRed;
                        button.ForeColor = Color.White;
                        button.Location = new Point(x + pictureBoxWidth + 10, y + label.Height + 10);
                        button.Size = new Size(100, 30);
                        button.Click += (s, e) =>
                        {
                            // show message box with image name
                            DialogResult dr = MessageBox.Show("Remove this content?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                                    var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=PABRChampions&ID=" + button.Name).Result;
                                    var resp = response.Content.ReadAsStringAsync();
                                    var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                                    var status = responseJson.status;
                                    var title = responseJson.title;

                                    if (status == "error" && title == "Data Not Deleted")
                                    {
                                        MessageBox.Show("Unable to delete champion.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else if (status == "success" && title == "Data Deleted")
                                    {
                                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    LoadChampions();
                                }
                            }
                        };
                        tabPage5.Controls.Add(button);

                        x += pictureBoxWidth + pictureBoxSpacing;
                        if ((i + 1) % pictureBoxsPerRow == 0)
                        {
                            x = 10;
                            y += pictureBoxHeight + 10; // add button height and spacing
                        }
                    }
                }
            }
            #endregion
        }

        void LoadTestimonials()
        {
            DataTable testimonial = new DataTable();
            BindingSource sbTestimonial = new BindingSource();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=Testimonial").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        testimonial.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = testimonial.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        testimonial.Rows.Add(row);
                    }

                    sbTestimonial.DataSource = testimonial;

                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = sbTestimonial;


                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Customer Name";
                    dataGridView1.Columns[2].HeaderText = "Review";
                    dataGridView1.Columns[3].HeaderText = "Date Reviewed";
                    dataGridView1.Columns[4].HeaderText = "Date Added";

                    dataGridView1.Columns[4].Visible = false; //PictureURL

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(deleteButton);

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    dataGridView1.Columns.Clear();
                    return;
                }
            }
        }

        void LoadStaff()
        {
            #region Clear Page
            while (tabPage4.Controls.Count > 0)
            {
                Control control = tabPage4.Controls[0];
                tabPage4.Controls.Remove(control);
                control.Dispose();
            }
            #endregion

            #region Fetch Details
            DataTable staff = new DataTable();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=PABRStaff").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        staff.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = staff.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        staff.Rows.Add(row);
                    }
                }

                if (staff.Rows.Count > 0)
                {
                    int x = 10;
                    int y = 30;
                    int pictureBoxWidth = 200;
                    int pictureBoxHeight = 200;
                    int pictureBoxSpacing = 250;
                    int pictureBoxsPerRow = 2;

                    for (int i = 0; i < staff.Rows.Count; i++)
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Load("https://pabrdex.com/contentimages/" + staff.Rows[i]["image"].ToString());
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox.Location = new Point(x, y);
                        pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
                        tabPage4.Controls.Add(pictureBox);

                        // Add label to display the image name on the right side of the picture box
                        Label label = new Label();
                        label.UseMnemonic = false;
                        label.Text = "Name: " + staff.Rows[i]["name"].ToString() +
                            "\nPosition: " + staff.Rows[i]["position"].ToString();
                        label.Location = new Point(x + pictureBoxWidth + 10, y);
                        label.AutoSize = true;
                        tabPage4.Controls.Add(label);

                        // Add button below the label
                        Button button = new Button();
                        button.Name = staff.Rows[i]["recID"].ToString();
                        button.Text = "Remove Staff";
                        button.BackColor = Color.IndianRed;
                        button.ForeColor = Color.White;
                        button.Location = new Point(x + pictureBoxWidth + 10, y + label.Height + 10);
                        button.Size = new Size(100, 30);
                        button.Click += (s, e) =>
                        {
                            // show message box with image name
                            DialogResult dr = MessageBox.Show("Remove staff?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                                    var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=PABRStaff&ID=" + button.Name).Result;
                                    var resp = response.Content.ReadAsStringAsync();
                                    var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                                    var status = responseJson.status;
                                    var title = responseJson.title;

                                    if (status == "error" && title == "Data Not Deleted")
                                    {
                                        MessageBox.Show("Unable to delete staff.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else if (status == "success" && title == "Data Deleted")
                                    {
                                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    LoadStaff();
                                }
                            }
                        };
                        tabPage4.Controls.Add(button);

                        x += pictureBoxWidth + pictureBoxSpacing;
                        if ((i + 1) % pictureBoxsPerRow == 0)
                        {
                            x = 10;
                            y += pictureBoxHeight + 10; // add button height and spacing
                        }
                    }
                }
            }
            #endregion
        }

        void LoadEventPhotos()
        {
            DataTable eventPhotos = new DataTable();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com/");
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.GetAsync("api/ContentData/GetContentData?RequestType=EventPhotos").Result;
                var resp = response.Content.ReadAsStringAsync();

                List<dynamic> jsonList = JsonConvert.DeserializeObject<List<dynamic>>(resp.Result);

                if (jsonList.Count > 0)
                {
                    //col
                    foreach (var item in jsonList[0])
                    {
                        eventPhotos.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = eventPhotos.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        eventPhotos.Rows.Add(row);
                    }
                }
                else
                {
                    return;
                }

                if (eventPhotos.Rows.Count > 0)
                {
                    webPaths = eventPhotos.AsEnumerable()
                        .Select(row => "https://pabrdex.com/contentimages/" + row.Field<string>("image"))
                        .ToArray();
                }
            }

            timer1.Start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

            button6.Text = "Add Record";

            LoadEventPoster();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

            button6.Text = "Add Record";

            LoadTestimonials();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

            button6.Text = "View Details";

            LoadEventPhotos();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;

            button6.Text = "Add Record";

            LoadStaff();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Form9 f9 = new Form9();
            f9.ShowDialog();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                button3.BackColor = Color.Silver;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.White;
                button7.BackColor = Color.White;

                button6.Text = "Add Record";

                LoadEventPoster();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.Silver;
                button2.BackColor = Color.White;
                button4.BackColor = Color.White;
                button7.BackColor = Color.White;

                button6.Text = "Add Record";

                LoadTestimonials();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.Silver;
                button4.BackColor = Color.White;
                button7.BackColor = Color.White;

                button6.Text = "View Details";

                LoadEventPhotos();
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.Silver;
                button7.BackColor = Color.White;

                button6.Text = "Add Record";

                LoadStaff();

            }
            else
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.White;
                button7.BackColor = Color.Silver;

                button6.Text = "Add Record";

                LoadChampions();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Form11 f11 = new Form11();
                f11.ShowDialog();
                LoadEventPoster();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Form15 f15 = new Form15();
                f15.ShowDialog();
                LoadTestimonials();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                Form17 f17 = new Form17();
                f17.ShowDialog();
                LoadEventPhotos();
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                Form16 f16 = new Form16();
                f16.ShowDialog();
                LoadStaff();
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                Form14 f14 = new Form14();
                f14.ShowDialog();
                LoadChampions();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;

            button6.Text = "Add Record";

            LoadChampions();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[5].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Delete this review?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                        var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=Testimonial&ID=" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()).Result;
                        var resp = response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                        var status = responseJson.status;
                        var title = responseJson.title;

                        if (status == "error" && title == "Data Not Deleted")
                        {
                            MessageBox.Show("Unable to delete review.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (status == "success" && title == "Data Deleted")
                        {
                            MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        LoadTestimonials();
                    }
                }
            }
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            // Set timer to tick every second
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;
            //label3.Text = label3.Text + "   " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update label with current time
            label3.Text = "Date and Time:    " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                currentIndex = (currentIndex + 1) % webPaths.Length;
                pictureBox2.Load(webPaths[currentIndex]);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex - 1 + webPaths.Length) % webPaths.Length;
            pictureBox2.Load(webPaths[currentIndex]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                button8.Text = "Play";
                button8.BackColor = Color.DimGray;
            }
            else
            {
                button8.Text = "Pause";
                button8.BackColor = Color.White;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex + 1) % webPaths.Length;
            pictureBox2.Load(webPaths[currentIndex]);
        }
    }
}

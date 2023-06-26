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
        public Form10()
        {
            InitializeComponent();

            tabControl1.SelectedIndex = 0;
            button3.BackColor = Color.Silver;
            //LoadEventPosterv2();

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
                    //webPaths = eventPhotos.AsEnumerable()
                    //    .Select(row => "https://pabrdex.com/contentimages/" + row.Field<string>("image"))
                    //    .ToArray();
                }
            }

            //timer1.Start();
        }

        void LoadEventPosterv2()
        {
            DataTable eventPoster = new DataTable();
            BindingSource SBeventPoster = new BindingSource();
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

                    SBeventPoster.DataSource = eventPoster;

                    dataGridView2.Columns.Clear();
                    dataGridView2.DataSource = SBeventPoster;


                    dataGridView2.Columns[0].HeaderText = "ID";
                    dataGridView2.Columns[1].HeaderText = "Title";
                    dataGridView2.Columns[2].HeaderText = "Schedule";
                    dataGridView2.Columns[3].HeaderText = "Location";
                    dataGridView2.Columns[4].HeaderText = "Description";
                    dataGridView2.Columns[5].HeaderText = "Image";
                    dataGridView2.Columns[6].HeaderText = "Date Added";

                    dataGridView2.Columns[5].Visible = false; //PictureURL
                    dataGridView2.Columns[6].Visible = false; //PictureURL

                    DataGridViewButtonColumn viewImageButton = new DataGridViewButtonColumn();
                    viewImageButton.HeaderText = "Image";
                    viewImageButton.Text = "View Image";
                    viewImageButton.UseColumnTextForButtonValue = true;
                    dataGridView2.Columns.Add(viewImageButton);

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView2.Columns.Add(deleteButton);

                    dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView2.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    dataGridView2.Columns.Clear();
                    return;
                }
            }
        }
        void LoadEventPhotosv2()
        {
            DataTable gallery = new DataTable();
            BindingSource sbGallery = new BindingSource();
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
                        gallery.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = gallery.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        gallery.Rows.Add(row);
                    }

                    sbGallery.DataSource = gallery;

                    dataGridView5.Columns.Clear();

                    // Add checkbox column to DataGridView
                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.HeaderText = "Select";
                    chk.Name = "chk";
                    chk.TrueValue = true;
                    chk.FalseValue = false;
                    dataGridView5.Columns.Insert(0, chk);

                    dataGridView5.DataSource = sbGallery;

                    dataGridView5.Columns[1].HeaderText = "ID";
                    dataGridView5.Columns[2].HeaderText = "File Name";
                    dataGridView5.Columns[3].HeaderText = "Date Added";

                    DataGridViewButtonColumn viewImageButton = new DataGridViewButtonColumn();
                    viewImageButton.HeaderText = "Image";
                    viewImageButton.Text = "View Image";
                    viewImageButton.UseColumnTextForButtonValue = true;
                    dataGridView5.Columns.Add(viewImageButton);

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "Action";
                    deleteButton.Name = "Delete";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView5.Columns.Add(deleteButton);

                    dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView5.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView5.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView5.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    dataGridView5.Columns.Clear();
                    return;
                }
            }
        }
        void LoadStaffv2()
        {
            DataTable pabrStaff = new DataTable();
            BindingSource SBpabrStaff = new BindingSource();
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
                        pabrStaff.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = pabrStaff.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        pabrStaff.Rows.Add(row);
                    }

                    SBpabrStaff.DataSource = pabrStaff;

                    dataGridView3.Columns.Clear();
                    dataGridView3.DataSource = SBpabrStaff;


                    dataGridView3.Columns[0].HeaderText = "ID";
                    dataGridView3.Columns[1].HeaderText = "Name";
                    dataGridView3.Columns[2].HeaderText = "Position";
                    dataGridView3.Columns[3].HeaderText = "Twitter";
                    dataGridView3.Columns[4].HeaderText = "Facebook";
                    dataGridView3.Columns[5].HeaderText = "Instagram";
                    dataGridView3.Columns[6].HeaderText = "Image";
                    dataGridView3.Columns[7].HeaderText = "Date Added";

                    dataGridView3.Columns[3].Visible = false; //
                    dataGridView3.Columns[4].Visible = false; //
                    dataGridView3.Columns[5].Visible = false; //
                    dataGridView3.Columns[6].Visible = false; //
                    dataGridView3.Columns[7].Visible = false; //

                    DataGridViewButtonColumn viewImageButton = new DataGridViewButtonColumn();
                    viewImageButton.HeaderText = "Image";
                    viewImageButton.Text = "View Image";
                    viewImageButton.UseColumnTextForButtonValue = true;
                    dataGridView3.Columns.Add(viewImageButton);

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView3.Columns.Add(deleteButton);

                    dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView3.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else
                {
                    dataGridView3.Columns.Clear();
                    return;
                }
            }
        }
        void LoadChampionsv2()
        {
            DataTable pabrChampion = new DataTable();
            BindingSource SBpabrChampionf = new BindingSource();
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
                        pabrChampion.Columns.Add(new DataColumn(item.Name, typeof(string)));
                    }

                    //row
                    foreach (var item in jsonList)
                    {
                        DataRow row = pabrChampion.NewRow();
                        foreach (var property in item)
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                        pabrChampion.Rows.Add(row);
                    }

                    SBpabrChampionf.DataSource = pabrChampion;

                    dataGridView4.Columns.Clear();
                    dataGridView4.DataSource = SBpabrChampionf;


                    dataGridView4.Columns[0].HeaderText = "ID";
                    dataGridView4.Columns[1].HeaderText = "Name";
                    dataGridView4.Columns[2].HeaderText = "Owner Name";
                    dataGridView4.Columns[3].HeaderText = "Image";
                    dataGridView4.Columns[4].HeaderText = "Certificate";
                    dataGridView4.Columns[5].HeaderText = "Date Added";

                    dataGridView4.Columns[3].Visible = false; //
                    dataGridView4.Columns[4].Visible = false; //
                    dataGridView4.Columns[5].Visible = false; //

                    DataGridViewButtonColumn viewImageButton = new DataGridViewButtonColumn();
                    viewImageButton.HeaderText = "Image";
                    viewImageButton.Text = "View Image";
                    viewImageButton.UseColumnTextForButtonValue = true;
                    dataGridView4.Columns.Add(viewImageButton);

                    DataGridViewButtonColumn viewCertificateButton = new DataGridViewButtonColumn();
                    viewCertificateButton.HeaderText = "Certificate";
                    viewCertificateButton.Text = "View Certificate";
                    viewCertificateButton.UseColumnTextForButtonValue = true;
                    dataGridView4.Columns.Add(viewCertificateButton);

                    DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                    deleteButton.HeaderText = "";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dataGridView4.Columns.Add(deleteButton);

                    dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView4.Columns.Clear();
                    return;
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

            button6.Text = "Add Record";

            //LoadEventPoster();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

            button6.Text = "Add Record";

            //LoadTestimonials();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

            button6.Text = "Add Record";

            //LoadEventPhotos();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;

            button6.Text = "Add Record";

            //LoadStaff();
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

                //LoadEventPoster();
                LoadEventPosterv2();
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

                //LoadEventPhotos();
                LoadEventPhotosv2();
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.Silver;
                button7.BackColor = Color.White;

                button6.Text = "Add Record";

                //LoadStaff();
                LoadStaffv2();

            }
            else
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.White;
                button7.BackColor = Color.Silver;

                button6.Text = "Add Record";

                //LoadChampions();
                LoadChampionsv2();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Form11 f11 = new Form11();
                f11.ShowDialog();
                //LoadEventPoster();
                LoadEventPosterv2();
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
                //LoadEventPhotos();
                LoadEventPhotosv2();
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                Form16 f16 = new Form16();
                f16.ShowDialog();
                //LoadStaff();
                LoadStaffv2();
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                Form14 f14 = new Form14();
                f14.ShowDialog();
                //LoadChampions();
                LoadChampionsv2();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;

            button6.Text = "Add Record";

            //LoadChampions();
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
            LoadEventPosterv2();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update label with current time
            label3.Text = "Date and Time:    " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns[8].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Delete this event?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                        var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=EventPoster&ID=" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString()).Result;
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

                        LoadEventPosterv2();
                    }
                }
            }
            else if (e.ColumnIndex == dataGridView2.Columns[7].Index && e.RowIndex >= 0)
            {
                CurSelectedContentImage.ContentImage = "";
                CurSelectedContentImage.ContentImage = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();

                Form18 f18 = new Form18();
                f18.ShowDialog();
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView3.Columns[9].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Remove this staff?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                        var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=PABRStaff&ID=" + dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString()).Result;
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

                        LoadStaffv2();
                    }
                }
            }
            else if (e.ColumnIndex == dataGridView3.Columns[8].Index && e.RowIndex >= 0)
            {
                CurSelectedContentImage.ContentImage = "";
                CurSelectedContentImage.ContentImage = dataGridView3.Rows[e.RowIndex].Cells[6].Value.ToString();

                Form18 f18 = new Form18();
                f18.ShowDialog();
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView4.Columns[8].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Remove this champion?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                        var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=PABRChampions&ID=" + dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString()).Result;
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

                        LoadChampionsv2();
                    }
                }
            }
            else if (e.ColumnIndex == dataGridView4.Columns[7].Index && e.RowIndex >= 0)
            {
                if (string.IsNullOrWhiteSpace(dataGridView4.Rows[e.RowIndex].Cells[4].Value.ToString()))
                {
                    CurSelectedContentImage.ContentImage = "";
                    MessageBox.Show("No Certificate Uploaded.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }

                CurSelectedContentImage.ContentImage = "";
                CurSelectedContentImage.ContentImage = dataGridView4.Rows[e.RowIndex].Cells[4].Value.ToString();

                Form19 f19 = new Form19();
                f19.ShowDialog();
            }
            else if (e.ColumnIndex == dataGridView4.Columns[6].Index && e.RowIndex >= 0)
            {
                CurSelectedContentImage.ContentImage = "";
                CurSelectedContentImage.ContentImage = dataGridView4.Rows[e.RowIndex].Cells[3].Value.ToString();

                Form18 f18 = new Form18();
                f18.ShowDialog();
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView5.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Prompt the user to confirm the deletion
                DialogResult result = MessageBox.Show("Delete this image?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool res = DeleteRecord(dataGridView5.Rows[e.RowIndex].Cells["recID"].Value.ToString());

                    if (res)
                    {
                        MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete image.\nPlease try again later.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadEventPhotosv2();
                }
            }
            else if (e.ColumnIndex == dataGridView5.Columns[4].Index && e.RowIndex >= 0)
            {
                CurSelectedContentImage.ContentImage = "";
                CurSelectedContentImage.ContentImage = dataGridView5.Rows[e.RowIndex].Cells[2].Value.ToString();

                Form19 f19 = new Form19();
                f19.ShowDialog();
            }
        }
        private bool DeleteRecord(string ID)
        {
            bool res = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://pabrdexapi.com");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + LoginDetails.accessToken);
                var response = httpClient.DeleteAsync("api/ContentData/DeleteContentData?RequestType=EventPhotos&ID=" + ID).Result;
                var resp = response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<dynamic>(resp.Result);
                var status = responseJson.status;
                var title = responseJson.title;

                if (status == "error" && title == "Data Not Deleted")
                {
                    res = false;
                }
                else if (status == "success" && title == "Data Deleted")
                {
                    res = true;
                }
            }

            return res;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            List<string> selectedIDs = new List<string>();
            foreach (DataGridViewRow row in dataGridView5.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells["chk"] as DataGridViewCheckBoxCell;
                if (chk.Value == chk.TrueValue)
                {
                    string id = row.Cells["recID"].Value.ToString();
                    selectedIDs.Add(id);
                }
            }

            if (selectedIDs.Count == 0)
            {
                MessageBox.Show("No selected records", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Delete selected image(s)?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (string value in selectedIDs)
                    {
                        DeleteRecord(value);
                    }
                    MessageBox.Show("Successfully deleted.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadEventPhotosv2();
                }
            }
        }
    }
}

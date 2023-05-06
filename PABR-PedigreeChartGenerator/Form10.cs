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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();

            tabControl1.SelectedIndex = 0;
            button3.BackColor = Color.Silver;

        }
        void test()
        {
            string[] imagePaths = Directory.GetFiles(@"C:\\Users\\BossMOMMY\\Downloads\\Bullies");
            int x = 10;
            int y = 50;
            int pictureBoxWidth = 200;
            int pictureBoxHeight = 300;
            int pictureBoxSpacing = 250;
            int pictureBoxsPerRow = 2;
            for (int i = 0; i < imagePaths.Length; i++)
            {
                Bitmap bmp = new Bitmap(imagePaths[i]);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = bmp;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Location = new Point(x, y);
                pictureBox.Size = new Size(pictureBoxWidth, pictureBoxHeight);
                tabPage1.Controls.Add(pictureBox);

                // Add label to display the image name on the right side of the picture box
                Label label = new Label();
                //label.Text = Path.GetFileName(imagePaths[i]);
                label.Text ="Name:" + Path.GetFileName(imagePaths[i]) +
                    "\nSaturday, February 25, 2023" +
                    "\nTiendesitas, Pasig City" +
                    "\nDog show for a cause.";
                //label.Location = new Point(x + pictureBoxWidth + pictureBoxSpacing, y);
                label.Location = new Point(x + pictureBoxWidth + 10, y);
                label.AutoSize = true;
                tabPage1.Controls.Add(label);

                // Add button below the label
                Button button = new Button();
                button.Name = Path.GetFileName(imagePaths[i]);
                button.Text = "Remove Event";
                button.BackColor = Color.IndianRed;
                button.ForeColor = Color.White;
                button.Location = new Point(x + pictureBoxWidth + 10, y + label.Height + 10);
                button.Size = new Size(label.Width, 30);
                button.Click += (s, e) => {
                    // show message box with image name
                    //string imageName = Path.GetFileName(imagePaths[i]);
                    DialogResult dr = MessageBox.Show("Remove " + button.Name + "?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                };
                tabPage1.Controls.Add(button);

                x += pictureBoxWidth + pictureBoxSpacing;
                if ((i + 1) % pictureBoxsPerRow == 0)
                {
                    x = 10;
                    //y += pictureBoxHeight + 10;
                    //y += pictureBoxHeight + label.Height + 10 * 2 + 30; // add button height and spacing
                    y += pictureBoxHeight + 10; // add button height and spacing
                }
            }



        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
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
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.Silver;
                button2.BackColor = Color.White;
                button4.BackColor = Color.White;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.Silver;
                button4.BackColor = Color.White;
            }
            else
            {
                button3.BackColor = Color.White;
                button1.BackColor = Color.White;
                button2.BackColor = Color.White;
                button4.BackColor = Color.Silver;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.ShowDialog();
            //test();
        }
    }
}

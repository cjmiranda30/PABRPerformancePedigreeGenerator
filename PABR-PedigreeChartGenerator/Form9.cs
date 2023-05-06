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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

            // Set timer to tick every second
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            label2.Text = label2.Text + "    " + LoginDetails.userFName + " " + LoginDetails.userLName;
            //label3.Text = label3.Text + "   " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

            this.Text = "PABR - MAIN MENU";
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update label with current time
            label3.Text = "Date and Time:    " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt");
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

        private void button3_Click(object sender, EventArgs e)
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();

            Form3 f3 = new Form3();
            f3.Show();
            this.Hide();
            this.Invoke(new Action(() => splash.Close()));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();

            Form10 f10 = new Form10();
            f10.Show();
            this.Hide();
            this.Invoke(new Action(() => splash.Close()));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();

            Form12 f12 = new Form12();
            f12.Show();
            this.Hide();
            this.Invoke(new Action(() => splash.Close()));
        }
    }
}

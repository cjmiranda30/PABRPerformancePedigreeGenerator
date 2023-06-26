namespace PABR_PedigreeChartGenerator
{
    partial class Form8
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            button3 = new Button();
            textBox2 = new TextBox();
            label2 = new Label();
            button2 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            button1 = new Button();
            groupBox2 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            progressBar1 = new ProgressBar();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Segoe UI Historic", 12F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(3, 1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(452, 124);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Historic", 8F, FontStyle.Italic, GraphicsUnit.Point);
            label4.ForeColor = Color.MediumBlue;
            label4.Location = new Point(73, 99);
            label4.Name = "label4";
            label4.Size = new Size(215, 13);
            label4.TabIndex = 19;
            label4.Text = "Kindly specify the folder path if available";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Historic", 8F, FontStyle.Italic, GraphicsUnit.Point);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(73, 55);
            label3.Name = "label3";
            label3.Size = new Size(142, 13);
            label3.TabIndex = 18;
            label3.Text = "Kindly specify the file path";
            // 
            // button3
            // 
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(413, 71);
            button3.Name = "button3";
            button3.Size = new Size(32, 25);
            button3.TabIndex = 17;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(72, 73);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(335, 23);
            textBox2.TabIndex = 16;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(19, 76);
            label2.Name = "label2";
            label2.Size = new Size(48, 15);
            label2.TabIndex = 15;
            label2.Text = "Images:";
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(414, 28);
            button2.Name = "button2";
            button2.Size = new Size(32, 25);
            button2.TabIndex = 14;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(73, 30);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(335, 23);
            textBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(9, 33);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 0;
            label1.Text = "CSV Path:";
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(296, 127);
            button1.Name = "button1";
            button1.Size = new Size(159, 56);
            button1.TabIndex = 12;
            button1.Text = "Upload";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(progressBar1);
            groupBox2.Location = new Point(3, 119);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(287, 64);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(6, 38);
            label6.Name = "label6";
            label6.Size = new Size(95, 15);
            label6.TabIndex = 21;
            label6.Text = "Status: Waiting...";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Historic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(248, 12);
            label5.Name = "label5";
            label5.Size = new Size(26, 15);
            label5.TabIndex = 20;
            label5.Text = "0 %";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(6, 12);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(236, 23);
            progressBar1.TabIndex = 0;
            // 
            // Form8
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(460, 194);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form8";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PABR - Batch Upload";
            Load += Form8_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Label label1;
        private Label label4;
        private Label label3;
        private Button button3;
        private TextBox textBox2;
        private Label label2;
        private GroupBox groupBox2;
        private Label label6;
        private Label label5;
        private ProgressBar progressBar1;
    }
}
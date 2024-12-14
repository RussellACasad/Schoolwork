namespace NavigationAtt2
{
    partial class FightScreen
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            textBox1 = new TextBox();
            progressBar1 = new ProgressBar();
            progressBar2 = new ProgressBar();
            textBox2 = new TextBox();
            button1 = new Button();
            panel2 = new Panel();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.border;
            pictureBox1.Location = new Point(12, 199);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(150, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.border;
            pictureBox2.Location = new Point(638, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(150, 150);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(620, 181);
            textBox1.TabIndex = 2;
            textBox1.Text = "The enemy attacks!";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 355);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(150, 25);
            progressBar1.TabIndex = 3;
            progressBar1.Value = 100;
            progressBar1.Click += progressBar1_Click;
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(638, 168);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(150, 25);
            progressBar2.TabIndex = 4;
            progressBar2.Value = 100;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(168, 199);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(464, 181);
            textBox2.TabIndex = 4;
            textBox2.Text = "Attack does this...";
            // 
            // button1
            // 
            button1.AllowDrop = true;
            button1.Dock = DockStyle.Top;
            button1.Location = new Point(0, 144);
            button1.Name = "button1";
            button1.Size = new Size(150, 36);
            button1.TabIndex = 5;
            button1.Text = "Confirm";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button5);
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Location = new Point(638, 199);
            panel2.Name = "panel2";
            panel2.Size = new Size(150, 181);
            panel2.TabIndex = 6;
            // 
            // button5
            // 
            button5.AllowDrop = true;
            button5.Dock = DockStyle.Top;
            button5.Location = new Point(0, 108);
            button5.Name = "button5";
            button5.Size = new Size(150, 36);
            button5.TabIndex = 3;
            button5.Text = "attack 4";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.AllowDrop = true;
            button4.Dock = DockStyle.Top;
            button4.Location = new Point(0, 72);
            button4.Name = "button4";
            button4.Size = new Size(150, 36);
            button4.TabIndex = 2;
            button4.Text = "attack 3";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.AllowDrop = true;
            button3.Dock = DockStyle.Top;
            button3.Location = new Point(0, 36);
            button3.Name = "button3";
            button3.Size = new Size(150, 36);
            button3.TabIndex = 1;
            button3.Text = "attack 2";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.AllowDrop = true;
            button2.Dock = DockStyle.Top;
            button2.Location = new Point(0, 0);
            button2.Name = "button2";
            button2.Size = new Size(150, 36);
            button2.TabIndex = 0;
            button2.Text = "attack 1";
            button2.UseVisualStyleBackColor = true;
            // 
            // FightScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(801, 394);
            Controls.Add(textBox2);
            Controls.Add(panel2);
            Controls.Add(progressBar2);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "FightScreen";
            Text = "FightScreen";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private TextBox textBox1;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private Button button1;
        private TextBox textBox2;
        private Panel panel2;
        private Button button5;
        private Button button4;
        private Button button3;
        private Button button2;
    }
}
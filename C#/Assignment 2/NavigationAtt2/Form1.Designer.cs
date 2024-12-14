namespace NavigationAtt2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtOutput = new TextBox();
            btnLeft = new Button();
            btnUp = new Button();
            btnRight = new Button();
            btnDown = new Button();
            btnAction = new Button();
            btnRestart = new Button();
            boxInventory = new GroupBox();
            btnInventory3 = new Button();
            btnInventory1 = new Button();
            btnInventory2 = new Button();
            boxCoords = new GroupBox();
            txtCoordY = new TextBox();
            txtCoordX = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtMap = new TextBox();
            label4 = new Label();
            label3 = new Label();
            boxInventory.SuspendLayout();
            boxCoords.SuspendLayout();
            SuspendLayout();
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(12, 12);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(399, 188);
            txtOutput.TabIndex = 1;
            txtOutput.Text = resources.GetString("txtOutput.Text");
            // 
            // btnLeft
            // 
            btnLeft.Enabled = false;
            btnLeft.Location = new Point(569, 167);
            btnLeft.Name = "btnLeft";
            btnLeft.Size = new Size(32, 32);
            btnLeft.TabIndex = 4;
            btnLeft.Text = "←";
            btnLeft.UseVisualStyleBackColor = true;
            btnLeft.Click += btnLeft_Click;
            // 
            // btnUp
            // 
            btnUp.Enabled = false;
            btnUp.Location = new Point(607, 129);
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(32, 32);
            btnUp.TabIndex = 5;
            btnUp.Text = "↑";
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // btnRight
            // 
            btnRight.Enabled = false;
            btnRight.Location = new Point(645, 167);
            btnRight.Name = "btnRight";
            btnRight.Size = new Size(32, 32);
            btnRight.TabIndex = 6;
            btnRight.Text = "→";
            btnRight.UseVisualStyleBackColor = true;
            btnRight.Click += btnRight_Click;
            // 
            // btnDown
            // 
            btnDown.Enabled = false;
            btnDown.Location = new Point(607, 167);
            btnDown.Name = "btnDown";
            btnDown.Size = new Size(32, 32);
            btnDown.TabIndex = 7;
            btnDown.Text = "↓";
            btnDown.UseVisualStyleBackColor = true;
            btnDown.Click += btnDown_Click;
            // 
            // btnAction
            // 
            btnAction.Enabled = false;
            btnAction.Location = new Point(569, 100);
            btnAction.Name = "btnAction";
            btnAction.Size = new Size(112, 23);
            btnAction.TabIndex = 8;
            btnAction.Text = "Action";
            btnAction.UseVisualStyleBackColor = true;
            btnAction.Click += btnAction_Click;
            // 
            // btnRestart
            // 
            btnRestart.Location = new Point(569, 71);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(112, 23);
            btnRestart.TabIndex = 9;
            btnRestart.Text = "Start";
            btnRestart.UseVisualStyleBackColor = true;
            btnRestart.Click += btnRestart_Click;
            // 
            // boxInventory
            // 
            boxInventory.Controls.Add(btnInventory3);
            boxInventory.Controls.Add(btnInventory1);
            boxInventory.Controls.Add(btnInventory2);
            boxInventory.Location = new Point(12, 206);
            boxInventory.Name = "boxInventory";
            boxInventory.Size = new Size(663, 76);
            boxInventory.TabIndex = 10;
            boxInventory.TabStop = false;
            boxInventory.Text = "Inventory";
            // 
            // btnInventory3
            // 
            btnInventory3.Enabled = false;
            btnInventory3.Location = new Point(440, 22);
            btnInventory3.Name = "btnInventory3";
            btnInventory3.Size = new Size(211, 48);
            btnInventory3.TabIndex = 14;
            btnInventory3.Text = "Slot 3";
            btnInventory3.UseVisualStyleBackColor = true;
            btnInventory3.Click += btnInventory3_Click;
            // 
            // btnInventory1
            // 
            btnInventory1.Enabled = false;
            btnInventory1.Location = new Point(6, 22);
            btnInventory1.Name = "btnInventory1";
            btnInventory1.Size = new Size(211, 48);
            btnInventory1.TabIndex = 12;
            btnInventory1.Text = "Slot 1";
            btnInventory1.UseVisualStyleBackColor = true;
            btnInventory1.Click += btnInventory1_Click;
            // 
            // btnInventory2
            // 
            btnInventory2.Enabled = false;
            btnInventory2.Location = new Point(223, 22);
            btnInventory2.Name = "btnInventory2";
            btnInventory2.Size = new Size(211, 48);
            btnInventory2.TabIndex = 13;
            btnInventory2.Text = "Slot 2";
            btnInventory2.UseVisualStyleBackColor = true;
            btnInventory2.Click += btnInventory2_Click;
            // 
            // boxCoords
            // 
            boxCoords.Controls.Add(txtCoordY);
            boxCoords.Controls.Add(txtCoordX);
            boxCoords.Location = new Point(569, 12);
            boxCoords.Name = "boxCoords";
            boxCoords.Size = new Size(112, 49);
            boxCoords.TabIndex = 11;
            boxCoords.TabStop = false;
            boxCoords.Text = "X/Y";
            // 
            // txtCoordY
            // 
            txtCoordY.Location = new Point(59, 20);
            txtCoordY.Name = "txtCoordY";
            txtCoordY.ReadOnly = true;
            txtCoordY.Size = new Size(47, 23);
            txtCoordY.TabIndex = 13;
            // 
            // txtCoordX
            // 
            txtCoordX.Location = new Point(6, 20);
            txtCoordX.Name = "txtCoordX";
            txtCoordX.ReadOnly = true;
            txtCoordX.Size = new Size(47, 23);
            txtCoordX.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(481, 130);
            label2.Name = "label2";
            label2.Size = new Size(13, 15);
            label2.TabIndex = 15;
            label2.Text = "S";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(479, 9);
            label1.Name = "label1";
            label1.Size = new Size(16, 15);
            label1.TabIndex = 14;
            label1.Text = "N";
            label1.Click += label1_Click;
            // 
            // txtMap
            // 
            txtMap.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtMap.Location = new Point(436, 27);
            txtMap.Multiline = true;
            txtMap.Name = "txtMap";
            txtMap.ReadOnly = true;
            txtMap.Size = new Size(102, 100);
            txtMap.TabIndex = 3;
            txtMap.Text = "g    g    g\r\n\r\ng    g    g\r\n\r\ng    g    g";
            txtMap.TextAlign = HorizontalAlignment.Center;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(544, 71);
            label4.Name = "label4";
            label4.Size = new Size(13, 15);
            label4.TabIndex = 17;
            label4.Text = "E";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(417, 71);
            label3.Name = "label3";
            label3.Size = new Size(18, 15);
            label3.TabIndex = 16;
            label3.Text = "W";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 292);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(txtMap);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(boxCoords);
            Controls.Add(boxInventory);
            Controls.Add(btnRestart);
            Controls.Add(btnAction);
            Controls.Add(btnDown);
            Controls.Add(btnRight);
            Controls.Add(btnUp);
            Controls.Add(btnLeft);
            Controls.Add(txtOutput);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            boxInventory.ResumeLayout(false);
            boxCoords.ResumeLayout(false);
            boxCoords.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtOutput;
        private Button btnLeft;
        private Button btnUp;
        private Button btnRight;
        private Button btnDown;
        private Button btnAction;
        private Button btnRestart;
        private GroupBox boxInventory;
        private GroupBox boxCoords;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button btnInventory1;
        private Button btnInventory2;
        private TextBox txtCoordY;
        private TextBox txtCoordX;
        private Button btnInventory3;
        private Label label2;
        private Label label1;
        private TextBox txtMap;
        private Label label4;
        private Label label3;
    }
}
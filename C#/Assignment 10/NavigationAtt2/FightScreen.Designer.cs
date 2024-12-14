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
            pbxHero = new PictureBox();
            pbxEnemy = new PictureBox();
            txtOutput = new TextBox();
            pgbHeroHP = new ProgressBar();
            pgbEnemyHP = new ProgressBar();
            txtAttackInfo = new TextBox();
            btnConfirm = new Button();
            panel2 = new Panel();
            rdoAttack4 = new RadioButton();
            rdoAttack3 = new RadioButton();
            rdoAttack2 = new RadioButton();
            rdoAttack1 = new RadioButton();
            lblPlayerHealth = new Label();
            ((System.ComponentModel.ISupportInitialize)pbxHero).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxEnemy).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pbxHero
            // 
            pbxHero.Image = Properties.Resources.border;
            pbxHero.Location = new Point(12, 199);
            pbxHero.Name = "pbxHero";
            pbxHero.Size = new Size(150, 150);
            pbxHero.SizeMode = PictureBoxSizeMode.Zoom;
            pbxHero.TabIndex = 0;
            pbxHero.TabStop = false;
            // 
            // pbxEnemy
            // 
            pbxEnemy.Image = Properties.Resources.border;
            pbxEnemy.Location = new Point(638, 12);
            pbxEnemy.Name = "pbxEnemy";
            pbxEnemy.Size = new Size(150, 150);
            pbxEnemy.SizeMode = PictureBoxSizeMode.Zoom;
            pbxEnemy.TabIndex = 1;
            pbxEnemy.TabStop = false;
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(12, 12);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(620, 181);
            txtOutput.TabIndex = 2;
            // 
            // pgbHeroHP
            // 
            pgbHeroHP.Location = new Point(12, 355);
            pgbHeroHP.Name = "pgbHeroHP";
            pgbHeroHP.Size = new Size(150, 12);
            pgbHeroHP.TabIndex = 3;
            pgbHeroHP.Value = 100;
            // 
            // pgbEnemyHP
            // 
            pgbEnemyHP.Location = new Point(638, 168);
            pgbEnemyHP.Name = "pgbEnemyHP";
            pgbEnemyHP.Size = new Size(150, 25);
            pgbEnemyHP.TabIndex = 4;
            pgbEnemyHP.Value = 100;
            // 
            // txtAttackInfo
            // 
            txtAttackInfo.Location = new Point(168, 199);
            txtAttackInfo.Multiline = true;
            txtAttackInfo.Name = "txtAttackInfo";
            txtAttackInfo.ReadOnly = true;
            txtAttackInfo.Size = new Size(513, 181);
            txtAttackInfo.TabIndex = 4;
            // 
            // btnConfirm
            // 
            btnConfirm.AllowDrop = true;
            btnConfirm.Dock = DockStyle.Bottom;
            btnConfirm.Location = new Point(0, 145);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(101, 36);
            btnConfirm.TabIndex = 5;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(rdoAttack4);
            panel2.Controls.Add(btnConfirm);
            panel2.Controls.Add(rdoAttack3);
            panel2.Controls.Add(rdoAttack2);
            panel2.Controls.Add(rdoAttack1);
            panel2.Location = new Point(687, 199);
            panel2.Name = "panel2";
            panel2.Size = new Size(101, 181);
            panel2.TabIndex = 6;
            // 
            // rdoAttack4
            // 
            rdoAttack4.AutoSize = true;
            rdoAttack4.Dock = DockStyle.Top;
            rdoAttack4.Location = new Point(0, 57);
            rdoAttack4.Name = "rdoAttack4";
            rdoAttack4.Size = new Size(101, 19);
            rdoAttack4.TabIndex = 12;
            rdoAttack4.Text = "Attack4";
            rdoAttack4.UseVisualStyleBackColor = true;
            rdoAttack4.Click += rdoAttack4_Click;
            // 
            // rdoAttack3
            // 
            rdoAttack3.AutoSize = true;
            rdoAttack3.Dock = DockStyle.Top;
            rdoAttack3.Location = new Point(0, 38);
            rdoAttack3.Name = "rdoAttack3";
            rdoAttack3.Size = new Size(101, 19);
            rdoAttack3.TabIndex = 11;
            rdoAttack3.Text = "Attack3";
            rdoAttack3.UseVisualStyleBackColor = true;
            rdoAttack3.Click += rdoAttack3_Click;
            // 
            // rdoAttack2
            // 
            rdoAttack2.AutoSize = true;
            rdoAttack2.Dock = DockStyle.Top;
            rdoAttack2.Location = new Point(0, 19);
            rdoAttack2.Name = "rdoAttack2";
            rdoAttack2.Size = new Size(101, 19);
            rdoAttack2.TabIndex = 9;
            rdoAttack2.Text = "Attack2";
            rdoAttack2.UseVisualStyleBackColor = true;
            rdoAttack2.Click += rdoAttack2_Click;
            // 
            // rdoAttack1
            // 
            rdoAttack1.AutoSize = true;
            rdoAttack1.Checked = true;
            rdoAttack1.Dock = DockStyle.Top;
            rdoAttack1.Location = new Point(0, 0);
            rdoAttack1.Name = "rdoAttack1";
            rdoAttack1.Size = new Size(101, 19);
            rdoAttack1.TabIndex = 10;
            rdoAttack1.TabStop = true;
            rdoAttack1.Text = "Attack1";
            rdoAttack1.UseVisualStyleBackColor = true;
            rdoAttack1.Click += rdoAttack1_Click;
            // 
            // lblPlayerHealth
            // 
            lblPlayerHealth.AutoSize = true;
            lblPlayerHealth.Location = new Point(12, 370);
            lblPlayerHealth.Name = "lblPlayerHealth";
            lblPlayerHealth.Size = new Size(47, 15);
            lblPlayerHealth.TabIndex = 8;
            lblPlayerHealth.Text = "HP: 100";
            // 
            // FightScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(801, 394);
            ControlBox = false;
            Controls.Add(lblPlayerHealth);
            Controls.Add(txtAttackInfo);
            Controls.Add(panel2);
            Controls.Add(pgbEnemyHP);
            Controls.Add(pgbHeroHP);
            Controls.Add(txtOutput);
            Controls.Add(pbxEnemy);
            Controls.Add(pbxHero);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FightScreen";
            Text = "FightScreen";
            Load += FightScreen_Load;
            ((System.ComponentModel.ISupportInitialize)pbxHero).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxEnemy).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbxHero;
        private PictureBox pbxEnemy;
        private TextBox txtOutput;
        private ProgressBar pgbHeroHP;
        private ProgressBar pgbEnemyHP;
        private Button btnConfirm;
        private TextBox txtAttackInfo;
        private Panel panel2;
        private Label lblPlayerHealth;
        private RadioButton rdoAttack4;
        private RadioButton rdoAttack3;
        private RadioButton rdoAttack2;
        private RadioButton rdoAttack1;
    }
}
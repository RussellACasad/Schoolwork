namespace Assignment1Casad
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
            txtOutput = new TextBox();
            txtInput = new TextBox();
            btnGo = new Button();
            SuspendLayout();
            // 
            // txtOutput
            // 
            txtOutput.Font = new Font("Cascadia Mono", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtOutput.Location = new Point(12, 12);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(249, 400);
            txtOutput.TabIndex = 0;
            txtOutput.TextAlign = HorizontalAlignment.Center;
            // 
            // txtInput
            // 
            txtInput.AccessibleDescription = "textbox to add a first and last initial for display";
            txtInput.AccessibleName = "initial textbox";
            txtInput.CharacterCasing = CharacterCasing.Upper;
            txtInput.Location = new Point(12, 418);
            txtInput.MaxLength = 2;
            txtInput.Name = "txtInput";
            txtInput.PlaceholderText = "RC";
            txtInput.Size = new Size(120, 23);
            txtInput.TabIndex = 1;
            txtInput.Text = "RC";
            // 
            // btnGo
            // 
            btnGo.Location = new Point(141, 418);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(120, 23);
            btnGo.TabIndex = 2;
            btnGo.Text = "Make it big!";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(273, 449);
            Controls.Add(btnGo);
            Controls.Add(txtInput);
            Controls.Add(txtOutput);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Assignment 1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtOutput;
        private Button btnGo;
        private TextBox txtInput;
    }
}
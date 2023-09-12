namespace SLAUCalc
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
            inTextBox = new TextBox();
            button1 = new Button();
            outTextBox = new TextBox();
            textBox1 = new TextBox();
            numericUpDown1 = new NumericUpDown();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // inTextBox
            // 
            inTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            inTextBox.BorderStyle = BorderStyle.FixedSingle;
            inTextBox.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            inTextBox.Location = new Point(12, 69);
            inTextBox.Multiline = true;
            inTextBox.Name = "inTextBox";
            inTextBox.PlaceholderText = "Введите систему уравнений";
            inTextBox.ScrollBars = ScrollBars.Both;
            inTextBox.Size = new Size(395, 407);
            inTextBox.TabIndex = 1;
            inTextBox.Text = "2x3=2\r\n4x1+x2=6\r\n4x1-3x2-5x3=1";
            inTextBox.WordWrap = false;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(413, 425);
            button1.Name = "button1";
            button1.Size = new Size(379, 46);
            button1.TabIndex = 2;
            button1.Text = "Решить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // outTextBox
            // 
            outTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            outTextBox.BorderStyle = BorderStyle.FixedSingle;
            outTextBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            outTextBox.Location = new Point(413, 69);
            outTextBox.Multiline = true;
            outTextBox.Name = "outTextBox";
            outTextBox.ReadOnly = true;
            outTextBox.ScrollBars = ScrollBars.Both;
            outTextBox.Size = new Size(379, 350);
            outTextBox.TabIndex = 3;
            outTextBox.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(12, 12);
            textBox1.MaxLength = 327670;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(311, 51);
            textBox1.TabIndex = 4;
            textBox1.Text = "x1,x2,x3";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDown1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            numericUpDown1.Location = new Point(329, 12);
            numericUpDown1.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(74, 33);
            numericUpDown1.TabIndex = 5;
            numericUpDown1.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(413, 12);
            button2.Name = "button2";
            button2.Size = new Size(379, 46);
            button2.TabIndex = 6;
            button2.Text = "Объявить переменные";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 478);
            Controls.Add(button2);
            Controls.Add(numericUpDown1);
            Controls.Add(textBox1);
            Controls.Add(outTextBox);
            Controls.Add(button1);
            Controls.Add(inTextBox);
            Name = "Form1";
            Text = "SLAUCalc";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox inTextBox;
        private Button button1;
        private TextBox outTextBox;
        private TextBox textBox1;
        private NumericUpDown numericUpDown1;
        private Button button2;
    }
}
namespace CryptoProject
{
    partial class OptionsForm
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
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(255, 192, 192);
            label6.Font = new Font("Bahnschrift", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(209, 29);
            label6.Name = "label6";
            label6.Size = new Size(28, 28);
            label6.TabIndex = 17;
            label6.Text = "%";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(255, 192, 192);
            label5.Font = new Font("Bahnschrift", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(209, 91);
            label5.Name = "label5";
            label5.Size = new Size(28, 28);
            label5.TabIndex = 16;
            label5.Text = "%";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(255, 192, 192);
            label4.BorderStyle = BorderStyle.Fixed3D;
            label4.Font = new Font("Bahnschrift", 12F);
            label4.Location = new Point(2, 62);
            label4.Name = "label4";
            label4.Size = new Size(273, 26);
            label4.TabIndex = 15;
            label4.Text = "Верхняя граница по спреду";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(255, 192, 192);
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Font = new Font("Bahnschrift", 12F);
            label3.Location = new Point(2, 0);
            label3.Name = "label3";
            label3.Size = new Size(267, 26);
            label3.TabIndex = 14;
            label3.Text = "Нижняя граница по спреду";
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(255, 192, 192);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Cursor = Cursors.Hand;
            textBox2.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox2.Location = new Point(2, 90);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(201, 29);
            textBox2.TabIndex = 13;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(255, 192, 192);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Cursor = Cursors.Hand;
            textBox1.Font = new Font("Bahnschrift", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.Location = new Point(2, 29);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(201, 29);
            textBox1.TabIndex = 12;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 192, 192);
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.ForeColor = SystemColors.ControlText;
            button2.Location = new Point(2, 122);
            button2.Name = "button2";
            button2.Size = new Size(267, 81);
            button2.TabIndex = 18;
            button2.Text = "Сохранить изменения";
            button2.UseVisualStyleBackColor = false;
            button2.Click += SaveButtonClick;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(477, 206);
            Controls.Add(button2);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "OptionsForm";
            Text = "Настройки";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button button2;
    }
}
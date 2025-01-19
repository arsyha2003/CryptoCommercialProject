namespace ArbiBot
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
            label1 = new Label();
            label2 = new Label();
            button2 = new Button();
            button1 = new Button();
            button3 = new Button();
            button4 = new Button();
            label4 = new Label();
            label3 = new Label();
            label6 = new Label();
            label5 = new Label();
            button5 = new Button();
            label7 = new Label();
            button6 = new Button();
            button7 = new Button();
            kryptonComboBox1 = new Krypton.Toolkit.KryptonComboBox();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)kryptonComboBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.SlateBlue;
            label1.Location = new Point(-2, -1);
            label1.Name = "label1";
            label1.Size = new Size(419, 617);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.BackColor = Color.DarkSlateBlue;
            label2.Location = new Point(-2, -1);
            label2.Name = "label2";
            label2.Size = new Size(419, 236);
            label2.TabIndex = 2;
            // 
            // button2
            // 
            button2.BackColor = Color.MediumSlateBlue;
            button2.FlatAppearance.BorderColor = Color.MidnightBlue;
            button2.FlatAppearance.BorderSize = 5;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Bahnschrift", 11F);
            button2.ForeColor = Color.Navy;
            button2.Location = new Point(-2, -1);
            button2.Name = "button2";
            button2.Padding = new Padding(4, 5, 4, 4);
            button2.Size = new Size(411, 64);
            button2.TabIndex = 3;
            button2.Text = "Перезапуск арби";
            button2.UseVisualStyleBackColor = false;
            button2.Click += StartArbitrageEvent;
            // 
            // button1
            // 
            button1.BackColor = Color.MediumSlateBlue;
            button1.FlatAppearance.BorderColor = Color.MidnightBlue;
            button1.FlatAppearance.BorderSize = 5;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Bahnschrift", 11F);
            button1.ForeColor = Color.Navy;
            button1.Location = new Point(-2, 69);
            button1.Name = "button1";
            button1.Padding = new Padding(4, 5, 4, 4);
            button1.Size = new Size(411, 64);
            button1.TabIndex = 4;
            button1.Text = "Перезапуск скринера";
            button1.UseVisualStyleBackColor = false;
            button1.Click += StartPumpAndDump;
            // 
            // button3
            // 
            button3.BackColor = Color.IndianRed;
            button3.FlatAppearance.BorderColor = Color.MidnightBlue;
            button3.FlatAppearance.BorderSize = 5;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Bahnschrift", 11F);
            button3.ForeColor = Color.Navy;
            button3.Location = new Point(-2, 159);
            button3.Name = "button3";
            button3.Padding = new Padding(4, 5, 4, 4);
            button3.Size = new Size(201, 76);
            button3.TabIndex = 7;
            button3.Text = "Остановка арби";
            button3.UseVisualStyleBackColor = false;
            button3.Click += StopArbitrageEvent;
            // 
            // button4
            // 
            button4.BackColor = Color.IndianRed;
            button4.FlatAppearance.BorderColor = Color.MidnightBlue;
            button4.FlatAppearance.BorderSize = 5;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Bahnschrift", 11F);
            button4.ForeColor = Color.Navy;
            button4.Location = new Point(205, 159);
            button4.Name = "button4";
            button4.Padding = new Padding(4, 5, 4, 4);
            button4.Size = new Size(204, 76);
            button4.TabIndex = 8;
            button4.Text = "Остановка скринера";
            button4.UseVisualStyleBackColor = false;
            button4.Click += StopPumpAndDumpBot;
            // 
            // label4
            // 
            label4.BackColor = Color.MediumSlateBlue;
            label4.Font = new Font("Bahnschrift", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(-2, 325);
            label4.Name = "label4";
            label4.Size = new Size(257, 60);
            label4.TabIndex = 10;
            label4.Text = "Статус скринера:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.BackColor = Color.MediumSlateBlue;
            label3.Font = new Font("Bahnschrift", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(-2, 245);
            label3.Name = "label3";
            label3.Size = new Size(257, 60);
            label3.TabIndex = 11;
            label3.Text = "Статус арбитражника:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.BackColor = Color.MediumSlateBlue;
            label6.Font = new Font("Bahnschrift", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(251, 325);
            label6.Name = "label6";
            label6.Size = new Size(132, 60);
            label6.TabIndex = 13;
            label6.Text = "Статус арбитражника";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.BackColor = Color.MediumSlateBlue;
            label5.Font = new Font("Bahnschrift", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(251, 245);
            label5.Name = "label5";
            label5.Size = new Size(132, 60);
            label5.TabIndex = 14;
            label5.Text = "Статус арбитражника";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button5
            // 
            button5.BackColor = Color.MediumSlateBlue;
            button5.FlatAppearance.BorderColor = Color.MidnightBlue;
            button5.FlatAppearance.BorderSize = 5;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Bahnschrift", 11F);
            button5.ForeColor = Color.Navy;
            button5.Location = new Point(-2, 516);
            button5.Name = "button5";
            button5.Padding = new Padding(4, 5, 4, 4);
            button5.Size = new Size(185, 100);
            button5.TabIndex = 15;
            button5.Text = "Настройка ограничений по спреду";
            button5.UseVisualStyleBackColor = false;
            button5.Click += OpenOptionsEvent;
            // 
            // label7
            // 
            label7.BackColor = Color.CornflowerBlue;
            label7.Font = new Font("Bahnschrift", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label7.Location = new Point(415, -1);
            label7.Name = "label7";
            label7.Size = new Size(566, 617);
            label7.TabIndex = 16;
            label7.Click += label7_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.MediumSlateBlue;
            button6.FlatAppearance.BorderColor = Color.MidnightBlue;
            button6.FlatAppearance.BorderSize = 5;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Bahnschrift", 11F);
            button6.ForeColor = Color.Navy;
            button6.Location = new Point(423, 505);
            button6.Name = "button6";
            button6.Padding = new Padding(4, 5, 4, 4);
            button6.Size = new Size(272, 100);
            button6.TabIndex = 18;
            button6.Text = "Показать пользователей";
            button6.UseVisualStyleBackColor = false;
            button6.Click += SelectAllRegisteredUsersEvent;
            // 
            // button7
            // 
            button7.BackColor = Color.MediumSlateBlue;
            button7.FlatAppearance.BorderColor = Color.MidnightBlue;
            button7.FlatAppearance.BorderSize = 5;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Bahnschrift", 11F);
            button7.ForeColor = Color.Navy;
            button7.Location = new Point(698, 505);
            button7.Name = "button7";
            button7.Padding = new Padding(4, 5, 4, 4);
            button7.Size = new Size(272, 100);
            button7.TabIndex = 19;
            button7.Text = "Удалить пользователей";
            button7.UseVisualStyleBackColor = false;
            // 
            // kryptonComboBox1
            // 
            kryptonComboBox1.CueHint.Color1 = Color.FromArgb(192, 192, 255);
            kryptonComboBox1.DropDownWidth = 259;
            kryptonComboBox1.IntegralHeight = false;
            kryptonComboBox1.ItemStyle = Krypton.Toolkit.ButtonStyle.Cluster;
            kryptonComboBox1.Location = new Point(436, 12);
            kryptonComboBox1.Name = "kryptonComboBox1";
            kryptonComboBox1.Size = new Size(534, 26);
            kryptonComboBox1.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonComboBox1.TabIndex = 20;
            kryptonComboBox1.Text = "Пользователи";
            // 
            // label8
            // 
            label8.BackColor = Color.MediumSlateBlue;
            label8.Font = new Font("Bahnschrift", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label8.Location = new Point(-2, 385);
            label8.Name = "label8";
            label8.Size = new Size(385, 60);
            label8.TabIndex = 21;
            label8.Text = "Статус арбитражника";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkOrchid;
            ClientSize = new Size(982, 612);
            Controls.Add(label8);
            Controls.Add(kryptonComboBox1);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(label7);
            Controls.Add(button5);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Form1";
            Load += OnFormLoadEvent;
            ((System.ComponentModel.ISupportInitialize)kryptonComboBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Label label2;
        private Button button2;
        private Button button1;
        private Button button3;
        private Button button4;
        private Label label4;
        private Label label3;
        private Label label6;
        private Label label5;
        private Button button5;
        private Label label7;
        private Button button6;
        private Button button7;
        private Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private Label label8;
    }
}

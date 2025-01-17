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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button2 = new Button();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            label6 = new Label();
            listBox1 = new ListBox();
            button3 = new Button();
            label3 = new Label();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            label1 = new Label();
            button1 = new Button();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 192, 192);
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.ForeColor = SystemColors.ControlText;
            button2.Location = new Point(12, 381);
            button2.Name = "button2";
            button2.Size = new Size(187, 61);
            button2.TabIndex = 5;
            button2.Text = "Запуск бота";
            button2.UseVisualStyleBackColor = false;
            button2.Click += StartArbitrageEvent;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 27);
            toolStrip1.TabIndex = 6;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(29, 24);
            toolStripButton1.Text = "OptionsButton";
            toolStripButton1.Click += OpenOptionsEvent;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(255, 192, 192);
            label6.Font = new Font("Bahnschrift", 12F);
            label6.Location = new Point(12, 62);
            label6.Name = "label6";
            label6.Size = new Size(94, 24);
            label6.TabIndex = 10;
            label6.Text = "тут текст";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(543, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(257, 124);
            listBox1.TabIndex = 11;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(255, 192, 192);
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button3.ForeColor = SystemColors.ControlText;
            button3.Location = new Point(543, 130);
            button3.Name = "button3";
            button3.Size = new Size(257, 37);
            button3.TabIndex = 12;
            button3.Text = "Показать пользователей";
            button3.UseVisualStyleBackColor = false;
            button3.Click += SelectAllRegisteredUsersEvent;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(255, 192, 192);
            label3.Font = new Font("Bahnschrift", 12F);
            label3.Location = new Point(248, 27);
            label3.Name = "label3";
            label3.Size = new Size(0, 24);
            label3.TabIndex = 13;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(255, 192, 192);
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button4.ForeColor = SystemColors.ControlText;
            button4.Location = new Point(205, 381);
            button4.Name = "button4";
            button4.Size = new Size(187, 61);
            button4.TabIndex = 14;
            button4.Text = "StartPumping";
            button4.UseVisualStyleBackColor = false;
            button4.Click += StartPumpAndDump;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(255, 192, 192);
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button5.ForeColor = SystemColors.ControlText;
            button5.Location = new Point(12, 314);
            button5.Name = "button5";
            button5.Size = new Size(187, 61);
            button5.TabIndex = 15;
            button5.Text = "Остановка бота";
            button5.UseVisualStyleBackColor = false;
            button5.Click += StopArbitrageEvent;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(255, 192, 192);
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button6.ForeColor = SystemColors.ControlText;
            button6.Location = new Point(205, 314);
            button6.Name = "button6";
            button6.Size = new Size(187, 61);
            button6.TabIndex = 16;
            button6.Text = "StopPumpingg";
            button6.UseVisualStyleBackColor = false;
            button6.Click += StopPumpAndDumpBot;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(255, 192, 192);
            label1.Font = new Font("Bahnschrift", 12F);
            label1.Location = new Point(248, 62);
            label1.Name = "label1";
            label1.Size = new Size(94, 24);
            label1.TabIndex = 17;
            label1.Text = "тут текст";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 192, 192);
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(543, 405);
            button1.Name = "button1";
            button1.Size = new Size(257, 37);
            button1.TabIndex = 18;
            button1.Text = "Очистить пользователей";
            button1.UseVisualStyleBackColor = false;
            button1.Click += ClearTable;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(listBox1);
            Controls.Add(label6);
            Controls.Add(toolStrip1);
            Controls.Add(button2);
            Name = "Form1";
            Text = "Form1";
            Load += OnFormLoadEvent;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private Label label6;
        private ListBox listBox1;
        private Button button3;
        private Label label3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Label label1;
        private Button button1;
    }
}

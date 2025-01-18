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
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            label3 = new Label();
            kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            kryptonButton2 = new Krypton.Toolkit.KryptonButton();
            kryptonButton3 = new Krypton.Toolkit.KryptonButton();
            kryptonButton4 = new Krypton.Toolkit.KryptonButton();
            kryptonButton5 = new Krypton.Toolkit.KryptonButton();
            kryptonButton6 = new Krypton.Toolkit.KryptonButton();
            kryptonListBox1 = new Krypton.Toolkit.KryptonListBox();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Font = new Font("Segoe UI", 9F);
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(910, 27);
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
            // kryptonButton1
            // 
            kryptonButton1.AccessibleRole = AccessibleRole.Chart;
            kryptonButton1.Location = new Point(499, 361);
            kryptonButton1.Name = "kryptonButton1";
            kryptonButton1.PaletteMode = Krypton.Toolkit.PaletteMode.SparklePurple;
            kryptonButton1.RightToLeft = RightToLeft.Yes;
            kryptonButton1.Size = new Size(199, 82);
            kryptonButton1.TabIndex = 19;
            kryptonButton1.Values.DropDownArrowColor = Color.Empty;
            kryptonButton1.Values.Text = "Перезапуск Абитражника";
            kryptonButton1.Click += StartArbitrageEvent;
            // 
            // kryptonButton2
            // 
            kryptonButton2.AccessibleRole = AccessibleRole.Chart;
            kryptonButton2.Location = new Point(704, 361);
            kryptonButton2.Name = "kryptonButton2";
            kryptonButton2.PaletteMode = Krypton.Toolkit.PaletteMode.SparklePurple;
            kryptonButton2.RightToLeft = RightToLeft.Yes;
            kryptonButton2.Size = new Size(199, 82);
            kryptonButton2.TabIndex = 20;
            kryptonButton2.Values.DropDownArrowColor = Color.Empty;
            kryptonButton2.Values.Text = "Перезапуск Скринера";
            kryptonButton2.Click += StartPumpAndDump;
            // 
            // kryptonButton3
            // 
            kryptonButton3.AccessibleRole = AccessibleRole.Chart;
            kryptonButton3.Location = new Point(499, 274);
            kryptonButton3.Name = "kryptonButton3";
            kryptonButton3.PaletteMode = Krypton.Toolkit.PaletteMode.SparklePurple;
            kryptonButton3.RightToLeft = RightToLeft.Yes;
            kryptonButton3.Size = new Size(199, 82);
            kryptonButton3.TabIndex = 21;
            kryptonButton3.Values.DropDownArrowColor = Color.Empty;
            kryptonButton3.Values.Text = "Остановка Арбитражника";
            kryptonButton3.Click += StopArbitrageEvent;
            // 
            // kryptonButton4
            // 
            kryptonButton4.AccessibleRole = AccessibleRole.Chart;
            kryptonButton4.Location = new Point(704, 274);
            kryptonButton4.Name = "kryptonButton4";
            kryptonButton4.PaletteMode = Krypton.Toolkit.PaletteMode.SparklePurple;
            kryptonButton4.RightToLeft = RightToLeft.Yes;
            kryptonButton4.Size = new Size(199, 82);
            kryptonButton4.TabIndex = 22;
            kryptonButton4.Values.DropDownArrowColor = Color.Empty;
            kryptonButton4.Values.Text = "Остановка Скринера";
            kryptonButton4.Click += StopPumpAndDumpBot;
            // 
            // kryptonButton5
            // 
            kryptonButton5.AccessibleRole = AccessibleRole.Chart;
            kryptonButton5.Location = new Point(499, 101);
            kryptonButton5.Name = "kryptonButton5";
            kryptonButton5.PaletteMode = Krypton.Toolkit.PaletteMode.SparkleOrangeDarkMode;
            kryptonButton5.RightToLeft = RightToLeft.Yes;
            kryptonButton5.Size = new Size(404, 65);
            kryptonButton5.TabIndex = 23;
            kryptonButton5.Values.DropDownArrowColor = Color.Empty;
            kryptonButton5.Values.Text = "Очистить список пользователей";
            kryptonButton5.Click += ClearTable;
            // 
            // kryptonButton6
            // 
            kryptonButton6.AccessibleRole = AccessibleRole.Chart;
            kryptonButton6.Location = new Point(499, 30);
            kryptonButton6.Name = "kryptonButton6";
            kryptonButton6.PaletteMode = Krypton.Toolkit.PaletteMode.SparkleOrangeDarkMode;
            kryptonButton6.RightToLeft = RightToLeft.Yes;
            kryptonButton6.Size = new Size(404, 65);
            kryptonButton6.TabIndex = 24;
            kryptonButton6.Values.DropDownArrowColor = Color.Empty;
            kryptonButton6.Values.Text = "Показать пользователей";
            kryptonButton6.Click += SelectAllRegisteredUsersEvent;
            // 
            // kryptonListBox1
            // 
            kryptonListBox1.BackStyle = Krypton.Toolkit.PaletteBackStyle.ContextMenuItemImage;
            kryptonListBox1.BorderStyle = Krypton.Toolkit.PaletteBorderStyle.ButtonCalendarDay;
            kryptonListBox1.Location = new Point(12, 30);
            kryptonListBox1.Name = "kryptonListBox1";
            kryptonListBox1.Size = new Size(434, 435);
            kryptonListBox1.TabIndex = 25;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Indigo;
            ClientSize = new Size(910, 450);
            Controls.Add(kryptonListBox1);
            Controls.Add(kryptonButton6);
            Controls.Add(kryptonButton5);
            Controls.Add(kryptonButton4);
            Controls.Add(kryptonButton3);
            Controls.Add(kryptonButton2);
            Controls.Add(kryptonButton1);
            Controls.Add(label3);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            Load += OnFormLoadEvent;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private Label label3;
        private Krypton.Toolkit.KryptonButton kryptonButton1;
        private Krypton.Toolkit.KryptonButton kryptonButton2;
        private Krypton.Toolkit.KryptonButton kryptonButton3;
        private Krypton.Toolkit.KryptonButton kryptonButton4;
        private Krypton.Toolkit.KryptonButton kryptonButton5;
        private Krypton.Toolkit.KryptonButton kryptonButton6;
        private Krypton.Toolkit.KryptonListBox kryptonListBox1;
    }
}

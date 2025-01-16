using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbiBot
{
    public partial class OptionsForm : Form
    {
        public Action<decimal, decimal> setRanges;
        public OptionsForm(Action<decimal,decimal>setRanges)
        {
            InitializeComponent();
            this.setRanges = setRanges;
        }
        public void SaveButtonClick(object sender, EventArgs e)
        {
            try
            {
                decimal range1 = decimal.Parse(textBox1.Text);
                decimal range2 = decimal.Parse(textBox2.Text);
                setRanges.Invoke(range1, range2);
                MessageBox.Show("Изменения сохранены"); 
                this.Close();
            }
            catch
            {
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                MessageBox.Show("Данные введены некорректно, введите данные в формате: 24,5");
            }
        }
    }
}

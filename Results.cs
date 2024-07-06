using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barriers
{
    public partial class Results : Form
    {
        public Results(string text)
        {
            InitializeComponent();
            label1.Text = text;
            FormClosing += (sender, e) => System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}

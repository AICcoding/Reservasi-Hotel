using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reservasi_Hotel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cari_no_kamar a = new cari_no_kamar();
            DialogResult dr = a.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("konden dadi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            check_in a = new check_in();
            DialogResult dr = a.ShowDialog();
        }
    }
}

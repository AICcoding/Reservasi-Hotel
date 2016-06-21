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
    public partial class detail_kamar : Form
    {
        public detail_kamar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ini merupakan rancangan tagihan untuk org terakhir (1 org)");
            pembayaran1 a = new pembayaran1();
            DialogResult dr = a.ShowDialog();

            MessageBox.Show("Ini merupakan rancangan tagihan untuk BUKAN org terakhir (1 org)");
            pembayaran2 b = new pembayaran2();
            DialogResult dr1 = b.ShowDialog();

            MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan MASIH ada org di kamar");
            pembayaran3 c = new pembayaran3();
            DialogResult dr2 = c.ShowDialog();

            MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan TIDAK ada org di kamar");
            pembayaran4 d = new pembayaran4();
            DialogResult dr3 = d.ShowDialog();
        }
    }
}

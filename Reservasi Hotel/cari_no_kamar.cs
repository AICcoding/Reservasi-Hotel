using MySql.Data.MySqlClient;
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
    public partial class cari_no_kamar : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        public cari_no_kamar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            detail_kamar a = new detail_kamar();
            DialogResult dr = a.ShowDialog();
        }
    }
}

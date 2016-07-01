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
            isi_kamar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string no_kamar = comboBox1.SelectedItem.ToString();
            if(cek_kamar_sudah_terisi(no_kamar)==true)
            {
                detail_kamar a = new detail_kamar();
                a.nomorKamar = Convert.ToInt32(comboBox1.SelectedItem.ToString());
                DialogResult dr = a.ShowDialog();
            }
            else
            {
                MessageBox.Show("Kamar " + no_kamar + " Kosong!", "Gagal check out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void isi_kamar()
        {
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("7");
            comboBox1.Items.Add("8");
            comboBox1.SelectedIndex = 0;
        }

        private bool cek_kamar_sudah_terisi(string id_kamar)
        {
            try
            {
                int jumlah_baris;
                string SQL = "SELECT COUNT(*) FROM reservasi WHERE id_kamar = " + id_kamar + " AND status_out=0;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                jumlah_baris = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if (jumlah_baris > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Reservasi_Hotel
{
    public partial class detail_kamar : Form
    {
        public int nomorKamar;
        int jumlahOrang;
        string totalTerbayar;
        DateTime tgl_check_in;
        DateTime jam_check_in;

        DateTime sekarang;

        int sisaBayar;

        public List<String> id_tamu;

        public detail_kamar()
        {
            InitializeComponent();
        }

        private void detail_kamar_Load(object sender, EventArgs e)
        {
            isiComboBox();
            isiDetail();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView2.Rows.Count > 0)
            {
                if (jumlahOrang == 1)
                {
                    MessageBox.Show("Ini merupakan rancangan tagihan untuk org terakhir (1 org)");
                    pembayaran1 a = new pembayaran1(nomorKamar, dataGridView2.Rows[0].Cells[0].Value.ToString());
                    DialogResult dr = a.ShowDialog();
                }
                else if (jumlahOrang > 1 && dataGridView2.Rows.Count == 1)
                {
                    MessageBox.Show("Ini merupakan rancangan tagihan untuk BUKAN org terakhir (1 org)");
                    pembayaran2 b = new pembayaran2();
                    DialogResult dr1 = b.ShowDialog();
                }
                else if (jumlahOrang > dataGridView2.Rows.Count && dataGridView2.Rows.Count > 1)
                {

                    MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan MASIH ada org di kamar");
                    pembayaran3 c = new pembayaran3();
                    DialogResult dr2 = c.ShowDialog();
                }
                else if (jumlahOrang == dataGridView2.Rows.Count)
                {

                    MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan TIDAK ada org di kamar");
                    pembayaran4 d = new pembayaran4();
                    DialogResult dr3 = d.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Silahkan pilih pelanggan yang check out terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void isiComboBox()
        {
            MySqlConnection conn = conectionservice.getconection();
            id_tamu = new List<String>();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaksi_tamu WHERE id_kamar = "+nomorKamar+ " AND tgl_check_out is null;", conn))
            {
                conn.Open();
                jumlahOrang = 0;
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    id_tamu.Add(dataReader.GetString(2));
                    comboBox1.Items.Add(dataReader.GetString(3));
                    jumlahOrang++;
                }
                conn.Close();
            }

            comboBox1.SelectedIndex = 0;
        }

        private void isiDetail()
        {
            MySqlConnection conn = conectionservice.getconection();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM reservasi WHERE id_kamar = " + nomorKamar + " AND status_out = 0;", conn))
            {
                conn.Open();
                //jumlahOrang = 0;
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    tgl_check_in = dataReader.GetDateTime(2);
                    jam_check_in = Convert.ToDateTime(dataReader.GetString(3));
                    totalTerbayar = dataReader.GetString(4);
                }
                conn.Close();
            }
            label5.Text = nomorKamar.ToString();
            label6.Text = tgl_check_in.ToString("dd-MM-yyyy") + " (" + jam_check_in.ToString("HH:mm") + ")";
            label7.Text = "Rp " + totalTerbayar.ToString() + ",-";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cek_sudah_ada_didaftar() == false)
            {
                dataGridView2.Rows.Add(id_tamu[comboBox1.SelectedIndex], comboBox1.SelectedItem);
            }
        }

        private bool cek_sudah_ada_didaftar()
        {
            bool status = false;
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if(dr.Cells[0].Value.ToString() == id_tamu[comboBox1.SelectedIndex])
                {
                    status = true;
                    break;
                }
            }
            return status;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentCell.RowIndex);
            }
            catch(Exception err)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
        }
    }
}

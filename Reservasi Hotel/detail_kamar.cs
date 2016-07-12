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

        MySqlConnection conn = conectionservice.getconection();
        MySqlCommand cmd;
        MySqlDataReader reader;
        string SQL;

        DateTime tgl_awal, tgl_akhir;

        int id_reservasi, tarif_kamar, temp_bayar, sisaBayar, lama_sewa, tarif_total, jumlah_extra_bed, lama_sewa_extra_bed;

        public List<String> id_tamu;

        public detail_kamar()
        {
            InitializeComponent();
        }

        private void detail_kamar_Load(object sender, EventArgs e)
        {
            isiComboBox();
            isiDetail();
            cari_id_transaksi_dan_reservasi();
            cari_total_bayar();
            cari_sisa_bayar();
            label10.Text = "Rp " + sisaBayar.ToString() + ",-";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView2.Rows.Count > 0)
            {
                if (jumlahOrang == 1)
                {
                    MessageBox.Show("Ini merupakan rancangan tagihan untuk org terakhir (1 org)");
                    pembayaran1 a = new pembayaran1(nomorKamar, dataGridView2.Rows[0].Cells[0].Value.ToString(), sisaBayar);
                    DialogResult dr = a.ShowDialog();
                }
                else if (jumlahOrang > 1 && dataGridView2.Rows.Count == 1)
                {
                    MessageBox.Show("Ini merupakan rancangan tagihan untuk BUKAN org terakhir (1 org)");
                    pembayaran2 b = new pembayaran2(nomorKamar, dataGridView2.Rows[0].Cells[0].Value.ToString(), Convert.ToInt32(label10.Text));
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
            id_tamu = new List<String>();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaksi_tamu WHERE id_kamar = " + nomorKamar + " AND tgl_keluar is null;", conn))
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
                    totalTerbayar = dataReader.GetString(6);
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

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ini merupakan rancangan tagihan untuk BUKAN org terakhir (1 org)");
            pembayaran2 b = new pembayaran2(nomorKamar, dataGridView2.Rows[0].Cells[0].Value.ToString(), Convert.ToInt32(sisaBayar));
            DialogResult dr1 = b.ShowDialog();
        }

        private void cari_id_transaksi_dan_reservasi()
        {
            try
            {
                SQL = "SELECT id_reservasi FROM reservasi WHERE id_kamar=" + nomorKamar + " AND status_out=0;";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_reservasi = Convert.ToInt32(reader.GetString("id_reservasi"));
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void jumlah_ekstra_bed(int nomor_kamar)
        {
            try
            {
                DateTime tgl_awal, tgl_akhir;
                tgl_awal = new DateTime(2013, 1, 13);
                tgl_akhir = new DateTime(2015, 1, 13);

                jumlah_extra_bed = 0;
                lama_sewa_extra_bed = 0;
                string SQL = "SELECT extra_bed.tgl_sewa, extra_bed.tgl_berhenti FROM extra_bed, reservasi WHERE reservasi.id_reservasi=extra_bed.id_reservasi AND reservasi.id_kamar='" + nomor_kamar + "'";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.IsDBNull(reader["tgl_berhenti"]))
                    {
                        tgl_awal = reader.GetDateTime("tgl_sewa");
                        tgl_akhir = DateTime.Now;
                    }
                    else
                    {
                        tgl_awal = reader.GetDateTime("tgl_sewa");
                        tgl_akhir = reader.GetDateTime("tgl_berhenti");
                    }


                    jumlah_extra_bed += 1;
                    lama_sewa_extra_bed += Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
                }
                conn.Close();

                tarif_total += (lama_sewa_extra_bed * jumlah_extra_bed * 50000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void cari_total_bayar()
        {
            SQL = "SELECT * FROM reservasi WHERE id_reservasi=" + id_reservasi + ";";
            conn.Open();
            cmd = new MySqlCommand(SQL, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tgl_awal = reader.GetDateTime("tgl_check_in");
                tgl_akhir = DateTime.Now;
            }
            conn.Close();

            SQL = "SELECT nominal FROM cek_tarif_kamar WHERE id_reservasi=" + id_reservasi + ";";
            conn.Open();
            cmd = new MySqlCommand(SQL, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tarif_kamar = reader.GetInt32("nominal");
            }
            conn.Close();

            jumlah_ekstra_bed(nomorKamar);
            lama_sewa = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
            lama_sewa++;
            tarif_total += (lama_sewa * tarif_kamar);
        }

        private void cari_sisa_bayar()
        {
            SQL = "SELECT temp_bayar FROM reservasi WHERE id_reservasi=" + id_reservasi + ";";
            conn.Open();
            cmd = new MySqlCommand(SQL, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                temp_bayar = reader.GetInt32("temp_bayar");
            }
            conn.Close();

            sisaBayar = tarif_total - temp_bayar;
        }
    }
}

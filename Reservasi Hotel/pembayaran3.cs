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
    public partial class pembayaran3 : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        int jumlah_extra_bed, lama_sewa_extra_bed, harga_total, id_reservasi, sisaBayar, temp_bayar, id_trx;
        string id_tamu_s;
        string tgl_c, jam_c;
        List<string> id_tamu = new List<string>();
        List<string> nama_tamu = new List<string>();

        int nomor_kamar;

        public pembayaran3(int nomor_kamar, List<string> id_tamu, List<string> nama_tamu, int sisaBayar)
        {
            InitializeComponent();
            this.nomor_kamar = nomor_kamar;
            this.id_tamu = id_tamu;
            this.nama_tamu = nama_tamu;
            this.sisaBayar = sisaBayar;
            label7.Text = "Rp " + sisaBayar.ToString() + ",-";
            
            isi_combo_box();
            init(this.nomor_kamar, this.id_tamu[comboBox1.SelectedIndex]);
        }
        private void init(int nomor_kamar, string id_tamu)
        {
            try
            {
                string tgl, jam;
                DateTime tgl_awal, tgl_akhir;
                tgl_awal = new DateTime(2013, 1, 13);
                tgl_akhir = new DateTime(2015, 1, 13);

                string SQL = "SELECT * FROM transaksi_tamu WHERE id_kamar=" + nomor_kamar + " AND id_tamu=" + id_tamu + ";";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label5.Text = reader.GetString("id_kamar");

                    tgl = reader.GetDateTime("tgl_masuk").ToString("yyyy-M-d");
                    jam = reader.GetTimeSpan("jam_masuk").ToString();
                    label6.Text = konversi_tgl_jam(tgl, jam);

                    tgl = DateTime.Now.ToString("yyyy-M-d");
                    jam = DateTime.Now.ToString("H:m:s");
                    label8.Text = konversi_tgl_jam(tgl, jam);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private string konversi_tgl_jam(string tgl, string jam)
        {
            string[] tmp_tgl = tgl.Split('-');
            string[] tmp_jam = jam.Split(':');
            string hasil_tgl, hasil_jam, hasil;
            hasil_jam = tmp_jam[0] + ":" + tmp_jam[1];
            hasil_tgl = tmp_tgl[2];
            int bulan = Convert.ToInt32(tmp_tgl[1]);
            switch (bulan)
            {
                case 1:
                    hasil_tgl += " Januari ";
                    break;
                case 2:
                    hasil_tgl += " Februari ";
                    break;
                case 3:
                    hasil_tgl += " Maret ";
                    break;
                case 4:
                    hasil_tgl += " April ";
                    break;
                case 5:
                    hasil_tgl += " Mei ";
                    break;
                case 6:
                    hasil_tgl += " Juni ";
                    break;
                case 7:
                    hasil_tgl += " Juli ";
                    break;
                case 8:
                    hasil_tgl += " Agustus ";
                    break;
                case 9:
                    hasil_tgl += " September ";
                    break;
                case 10:
                    hasil_tgl += " Oktober ";
                    break;
                case 11:
                    hasil_tgl += " November ";
                    break;
                case 12:
                    hasil_tgl += " Desember ";
                    break;
            }
            hasil_tgl += tmp_tgl[0];
            hasil = hasil_tgl + " (" + hasil_jam + ")";
            return hasil;
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

                label12.Text = jumlah_extra_bed.ToString() + " bed (" + lama_sewa_extra_bed + " hari)";
                harga_total += (lama_sewa_extra_bed * jumlah_extra_bed * 50000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void cari_id_transaksi_dan_reservasi(int nomor_kamar, string id_tamu)
        {
            try
            {
                string SQL = "SELECT id_reservasi FROM reservasi WHERE id_kamar=" + nomor_kamar + " AND status_out=0;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_reservasi = Convert.ToInt32(reader.GetString("id_reservasi"));
                }
                conn.Close();

                SQL = "SELECT id_transaksi FROM transaksi WHERE id_reservasi=" + id_reservasi + " AND id_tamu=" + id_tamu + ";";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_trx = Convert.ToInt32(reader.GetString("id_transaksi"));
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void isi_combo_box()
        {
            foreach(string item in nama_tamu)
            {
                comboBox1.Items.Add(item);
            }

            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch
            {
                comboBox1.SelectedIndex = -1;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cari_id_transaksi_dan_reservasi(nomor_kamar, id_tamu[comboBox1.SelectedIndex]);

            if (int.Parse((textBox1.Text)) > sisaBayar)
            {
                MessageBox.Show("Silahkan periksa kembali nominal pembayaran!");
            }
            else
            {
                try
                {
                    string tgl, jam;
                    tgl = DateTime.Now.ToString("yyyy-M-d");
                    jam = DateTime.Now.ToString("H:m:s");
                    string SQL = "UPDATE transaksi SET tgl_keluar='" + tgl + "', jam_keluar='" + jam + "', jumlah_bayar='" + textBox1.Text + "' WHERE id_transaksi=" + id_trx + ";";
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                    }
                    conn.Close();

                    SQL = "UPDATE reservasi SET temp_bayar= temp_bayar + '" + textBox1.Text + "' WHERE id_reservasi=" + id_reservasi + ";";
                    MessageBox.Show(SQL);
                    conn.Open();
                    cmd = new MySqlCommand(SQL, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                    }
                    conn.Close();

                    MessageBox.Show("Berhasil melakukan pembayaran!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sisaBayar -= int.Parse(textBox1.Text);
                    label7.Text = "Rp " + sisaBayar.ToString() + ",-";

                    id_tamu.RemoveAt(comboBox1.SelectedIndex);
                    nama_tamu.RemoveAt(comboBox1.SelectedIndex);
                    comboBox1.Items.Clear();

                    isi_combo_box();
                    
                    if(comboBox1.Items.Count == 0)
                    {
                        button1.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    conn.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            init(this.nomor_kamar, this.id_tamu[comboBox1.SelectedIndex]);
        }
    }
}

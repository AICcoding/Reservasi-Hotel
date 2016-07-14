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

        //rekomendasi
        public List<DateTime> tgl_masuk_tamu = new List<DateTime>();
        public List<string> nama = new List<string>();
        public List<int> tarif = new List<int>();
        List<int> mark = new List<int>();

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
            rekomendasi();
            label10.Text = "Rp " + format_idr(sisaBayar.ToString()) + ",-";
            isi_yang_sudah_bayar();
        }

        private void isi_yang_sudah_bayar()
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaksi_tamu WHERE id_kamar = " + nomorKamar + " AND status_check_out = 1;", conn))
            {
                conn.Open();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                int i = -1;
                while (dataReader.Read())
                {
                    i += 1;
                    dataGridView1.Rows.Add(1);
                    dataGridView1.Rows[i].Cells[0].Value = dataReader.GetString("nama");
                    dataGridView1.Rows[i].Cells[1].Value = dataReader.GetDateTime("tgl_keluar").ToString("d-M-yyyy");
                    dataGridView1.Rows[i].Cells[2].Value = dataReader.GetString("jumlah_bayar");
                }
                conn.Close();
            }
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
                    pembayaran2 b = new pembayaran2(nomorKamar, dataGridView2.Rows[0].Cells[0].Value.ToString(), sisaBayar);
                    DialogResult dr1 = b.ShowDialog();
                }
                else if (jumlahOrang > dataGridView2.Rows.Count && dataGridView2.Rows.Count > 1)
                {

                    MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan MASIH ada org di kamar");
                    List<string> id_tamu = new List<string>();
                    List<string> nama_tamu = new List<string>();
                    for(int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        id_tamu.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                        nama_tamu.Add(dataGridView2.Rows[i].Cells[1].Value.ToString());
                    }
                    pembayaran3 c = new pembayaran3(nomorKamar, id_tamu, nama_tamu, sisaBayar);
                    DialogResult dr2 = c.ShowDialog();
                }
                else if (jumlahOrang == dataGridView2.Rows.Count)
                {
                    MessageBox.Show("Ini merupakan rancangan tagihan untuk pelanggan yg barengan check out dan TIDAK ada org di kamar");
                    List<string> id_tamu = new List<string>();
                    List<string> nama_tamu = new List<string>();
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        id_tamu.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                        nama_tamu.Add(dataGridView2.Rows[i].Cells[1].Value.ToString());
                    }

                    pembayaran4 d = new pembayaran4(nomorKamar, id_tamu, nama_tamu, sisaBayar);
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
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaksi_tamu WHERE id_kamar = " + nomorKamar + " AND status_check_out = 0;", conn))
            {
                conn.Open();
                jumlahOrang = 0;
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    id_tamu.Add(dataReader.GetString("id_tamu"));
                    comboBox1.Items.Add(dataReader.GetString("nama"));
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
            label7.Text = "Rp " + format_idr(totalTerbayar.ToString()) + ",-";
        }

        private string format_idr(string input)
        {
            int hitung;
            string tmp, hasil_nominal;
            char[] tmp_input;

            tmp = input;
            tmp = tmp.Replace(".", "");
            hasil_nominal = "";
            hitung = 2 - ((tmp.Length) % 3);
            tmp_input = tmp.ToCharArray();

            foreach (char karakter in tmp_input)
            {
                if (hitung == 2)
                {
                    if (hasil_nominal == "")
                    {
                        hasil_nominal += karakter;
                        hitung = 0;
                    }
                    else
                    {
                        hasil_nominal += "." + karakter;
                        hitung = 0;
                    }
                }
                else
                {
                    hasil_nominal += karakter;
                    hitung += 1;
                }
            }
            return hasil_nominal;
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
                string SQL = "SELECT extra_bed.tgl_sewa, extra_bed.tgl_berhenti, nominal, status_selesai FROM extra_bed, reservasi, tarif WHERE reservasi.id_reservasi=extra_bed.id_reservasi AND extra_bed.id_tarif=tarif.id_tarif AND reservasi.id_kamar='" + nomor_kamar + "' AND status_out=0";

                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if(reader.GetString("status_selesai")=="1")
                    {
                        tgl_awal = reader.GetDateTime("tgl_sewa");
                        tgl_akhir = reader.GetDateTime("tgl_berhenti");
                        lama_sewa_extra_bed = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
                        tarif_total += (lama_sewa_extra_bed * Convert.ToInt32(reader.GetString("nominal")));
                    }
                    else
                    {
                        tgl_awal = reader.GetDateTime("tgl_sewa");
                        tgl_akhir = DateTime.Now;
                        lama_sewa_extra_bed = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
                        tarif_total += (lama_sewa_extra_bed * Convert.ToInt32(reader.GetString("nominal")));
                    }              
                }
                conn.Close();
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

            //ubah_status_extra_bed(id_reservasi.ToString());
            jumlah_ekstra_bed(nomorKamar);
            lama_sewa = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
            tarif_total += (lama_sewa * tarif_kamar);
        }

        private void ubah_status_extra_bed(string id_reservasi)
        {
            try
            {
                string SQL = "UPDATE extra_bed SET status_selesai=1, tgl_berhenti='" + DateTime.Now.ToString("yyyy-M-d") +"' WHERE id_reservasi='" + id_reservasi + "';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
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

        private void rekomendasi()
        {
            SQL = "SELECT * FROM rekomendasi WHERE id_reservasi=" + id_reservasi + ";";
            conn.Open();
            cmd = new MySqlCommand(SQL, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama.Add(reader.GetString("nama"));
                tgl_masuk_tamu.Add(reader.GetDateTime("tgl_masuk"));
                tarif.Add(0);
                mark.Add(0);
            }
            conn.Close();
            tgl_akhir = new DateTime(2016, 7, 20, 16,0,0);

            int range = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
            //MessageBox.Show(range.ToString());

            int jumlah; //Jumlah orang pada tanggal x
            for(int i = 0; i < range; i++)
            {
                jumlah = 0;
                for(int j = 0; j < nama.Count; j++)
                {
                    if((tgl_awal.AddDays(i) >= tgl_masuk_tamu[j]))
                    {
                        jumlah++;
                        mark[j] = 1;
                    }
                }
                //MessageBox.Show("tanggal segini: " + tgl_awal.AddDays(i).ToString() + " terdapat " + jumlah + " orang");

                for (int j = 0; j < nama.Count; j++)
                {
                    if(mark[j] == 1)
                    {
                        tarif[j] += tarif_kamar / jumlah;
                        mark[j] = 0;
                    }

                    //MessageBox.Show("tanggal segini: " + tgl_awal.AddDays(i).ToString() + " si " + nama[j] + " bayar " + tarif[j]);
                }
                
            }

            
        }
   
    }
}

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
    public partial class check_in : Form
    {
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["detail_tamu_satu_kamar"];
        MySqlConnection conn = conectionservice.getconection();

        string tgl, jam, id_kamar, id_tamu_lama, id_tamu_baru;
        bool sudah_mengetik, status_check_in;

        public check_in()
        {
            InitializeComponent();
            label6.Text = "";
            sudah_mengetik = false;
            status_check_in = false;
            dateTimePicker1.Value = DateTime.Now.AddDays(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_inn();
        }

        private bool cek_tamu_sudah_ada_di_db()
        {
            try
            {
                string id;
                id = textBox2.Text;
                id = id.Trim();

                string SQL = "SELECT COUNT(*) FROM tamu WHERE id_tamu='"+id+"'";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if(count > 0)
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

        private void check_inn()
        {
            //---------
            tgl = DateTime.Now.ToString("yyyy-M-d");
            jam = DateTime.Now.ToString("H:m:s");
            id_kamar = textBox1.Text;
            //---------

            try
            {
                string id, no_telp, nama, alamat, nomor_kamar, tgl_check_out;
                id = textBox2.Text;
                id = id.Trim();
                no_telp = textBox4.Text;
                no_telp = no_telp.Trim();
                nama = textBox3.Text;
                nama = nama.Trim();
                alamat = textBox5.Text;
                alamat = alamat.Trim();
                tgl_check_out = dateTimePicker1.Value.ToString("yyyy-M-d");

                nomor_kamar = textBox1.Text;

                DateTime awal, akhir;
                String[] tmp_tgl = DateTime.Now.ToString("yyyy-M-d").Split('-');
                awal = new DateTime(int.Parse(tmp_tgl[0]), int.Parse(tmp_tgl[1]), int.Parse(tmp_tgl[2]));
                tmp_tgl = tgl_check_out.Split('-');
                akhir = new DateTime(int.Parse(tmp_tgl[0]), int.Parse(tmp_tgl[1]), int.Parse(tmp_tgl[2]));


                if (id == "" || no_telp == "" || nama == "" || alamat == "" || nomor_kamar == "")
                {
                    MessageBox.Show("Anda belum memasukkan semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if ((akhir - awal).TotalDays < 1)
                {
                    MessageBox.Show("Check out minimal 1 hari setelah check in!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (cek_tamu_sudah_ada_di_db() == true)
                    {
                        update_tamu(id);
                    }
                    else
                    {
                        tambah_tamu(id, nama, alamat, no_telp);
                    }   
                 
                    //cek reservasi sudah ada atau belum
                    //tambah reservasi jika belum
                    //buat transaksi yang mengacu ke id reservasi
                    if(sudah_ada_reservasi(nomor_kamar)==false)
                    {
                        tambah_reservasi(nomor_kamar);
                        tambah_transaksi(nomor_kamar, id);
                        update_tgl_out_reservasi(nomor_kamar);
                    }
                    else
                    {
                        tambah_transaksi(nomor_kamar, id);
                        update_tgl_out_reservasi(nomor_kamar);
                    }

                    if(status_check_in==true)
                    {
                        MessageBox.Show("Check in berhasil!\n\nNomor kamar: " + nomor_kamar, "Berhasil check in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hapus_form();
                        status_check_in = false;
                    }
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void tambah_reservasi(string id_kamar)
        {
            try
            {
                string tgl_sekarang, jam_sekarang, tgl_out, jam_out;
                tgl_sekarang = DateTime.Now.ToString("yyyy-M-d");
                jam_sekarang = DateTime.Now.ToString("H:m:s");
                tgl_out = dateTimePicker1.Value.ToString("yyyy-M-d");
                jam_out = "06:00:00";

                string SQL = "INSERT INTO reservasi (id_kamar, tgl_check_in, jam_check_in, tgl_check_out, jam_check_out, temp_bayar, status_out) VALUES ('" + id_kamar + "','" + tgl_sekarang + "','" + jam_sekarang + "', '" + tgl_out + "', '" + jam_out + "', 0, 0);";
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

        private void tambah_transaksi(string id_kamar, string id_tamu)
        {
            try
            {
                string tgl_sekarang, jam_sekarang, id_reservasi, tgl_out, jam_out;
                tgl_sekarang = DateTime.Now.ToString("yyyy-M-d");
                jam_sekarang = DateTime.Now.ToString("H:m:s");
                tgl_out = dateTimePicker1.Value.ToString("yyyy-M-d");
                jam_out = DateTime.Now.ToString("H:m:s");
                id_reservasi = "";

                string SQL = "SELECT id_reservasi FROM reservasi WHERE id_kamar="+id_kamar+" AND status_out=0 ORDER BY id_reservasi DESC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_reservasi = reader.GetString("id_reservasi");
                    break;
                }
                conn.Close();

                SQL = "INSERT INTO transaksi (id_reservasi, id_tamu, tgl_masuk, jam_masuk, tgl_keluar, jam_keluar, jumlah_bayar) VALUES ('" + id_reservasi + "', '" + id_tamu + "', '" + tgl_sekarang + "', '" + jam_sekarang + "', '" + tgl_out + "', '" + jam_out + "', 0);";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                conn.Close();
                status_check_in = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        
        private void update_tgl_out_reservasi(string id_kamar)
        {            
            try
            {
                DateTime tgl_out_baru, tgl_out_lama;          
                tgl_out_baru = dateTimePicker1.Value;
                tgl_out_lama = dateTimePicker1.Value;

                TimeSpan jam_out_lama, jam_out_baru;
                jam_out_lama = DateTime.Now.TimeOfDay;
                jam_out_baru = DateTime.Now.TimeOfDay;

                string SQL = "SELECT tgl_check_out, jam_check_out FROM reservasi WHERE id_kamar=" + id_kamar + " AND status_out=0 ORDER BY id_reservasi DESC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tgl_out_lama = reader.GetDateTime("tgl_check_out");
                    jam_out_lama = reader.GetTimeSpan("jam_check_out");
                    break;
                }
                conn.Close();

                if ((tgl_out_baru - tgl_out_lama).TotalDays > 0)
                {
                    SQL = "UPDATE reservasi set tgl_check_out='" + tgl_out_baru.ToString("yyyy-M-d") + "' WHERE id_kamar=" + id_kamar + " AND status_out=0;";
                    conn.Open();
                    cmd = new MySqlCommand(SQL, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
                    }
                    conn.Close();
                }
                
                /*
                if (Convert.ToDouble((jam_out_baru - jam_out_lama).TotalHours) > 0F)
                {
                    SQL = "UPDATE reservasi set jam_check_out='" + jam_out_baru.ToString("H:m:s") + "' WHERE id_kamar=" + id_kamar + " AND status_out=0;";
                    conn.Open();
                    cmd = new MySqlCommand(SQL, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                    }
                    conn.Close();
                } */   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private bool sudah_ada_reservasi(string no_kamar)
        {
            try
            {
                string id;
                id = textBox2.Text;
                id = id.Trim();

                string SQL = "SELECT COUNT(*) FROM reservasi WHERE id_kamar='" + no_kamar + "' AND status_out=0;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if (count > 0)
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

        private void tambah_tamu(string id, string nama, string alamat, string no_telp)
        {
            try
            {
                string SQL = "INSERT INTO tamu (id_tamu, nama, alamat, telepon) VALUES ('" + id + "','" + nama + "','" + alamat + "','" + no_telp + "');";
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

        private void update_tamu(string id_tamu)
        {
            try
            {
                string nama, alamat, no_telp;
                nama = textBox3.Text;
                alamat = textBox5.Text;
                no_telp = textBox4.Text;
                string SQL = "UPDATE tamu set nama='" + nama + "', alamat='" + alamat + "', telepon='" + no_telp + "' WHERE id_tamu='" + id_tamu + "';";
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

        private void hapus_form()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            label6.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var form = new pilih_kamar_2())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox1.Text = form.ReturnValue1;
                }
            }
        }   

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            sudah_mengetik = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(sudah_mengetik==true)
            {
                id_tamu_lama = id_tamu_baru;
                id_tamu_baru = textBox2.Text;
                if(id_tamu_lama!=id_tamu_baru)
                {
                    try
                    {
                        bool ada_baris = false;
                        string SQL = "SELECT * FROM tamu WHERE id_tamu = '" + id_tamu_baru + "';";
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(SQL, conn);
                        cmd = new MySqlCommand(SQL, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            label6.ForeColor = Color.Green;
                            label6.Text = "*Data tamu sudah tersimpan!.";

                            textBox3.Text = reader.GetString("nama");
                            textBox4.Text = reader.GetString("telepon");
                            textBox5.Text = reader.GetString("alamat");
                            ada_baris = true;
                            break;
                        }
                        if(ada_baris==false)
                        {
                            label6.ForeColor = Color.Red;
                            label6.Text = "*Data tamu belum ada! Silahkan memasukkannya secara manual.";

                            textBox3.Text = "";
                            textBox4.Text = "";
                            textBox5.Text = "";
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        conn.Close();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }        
    }
}

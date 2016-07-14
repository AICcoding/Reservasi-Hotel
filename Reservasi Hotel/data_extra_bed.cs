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
    public partial class data_extra_bed : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        string keterangan;
        List<string> id_tarif;

        public data_extra_bed(string ket)
        {
            InitializeComponent();
            keterangan = ket;
            id_tarif = new List<string>();
            init();
        }

        private void init()
        {            
            if(keterangan=="tambah")
            {
                this.Text = "Tambah extra bed";
                label1.Text = "TAMBAH EXTRA BED";
                toolTip1.SetToolTip(button1, "Tambah extra bed");

                isi_combo_box1();       
            }
            else
            {
                this.Text = "Edit extra bed";
                label1.Text = "EDIT EXTRA BED";
                toolTip1.SetToolTip(button1, "Edit extra bed");

                isi_combo_box2();
                comboBox2.Enabled = false;
            }          
        }

        private void isi_combo_box1()
        {
            try
            {
                string SQL = "SELECT id_tarif, nama_tarif, nominal FROM tarif WHERE nama_tarif LIKE '%bed%' ORDER BY nama_tarif ASC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                id_tarif.Clear();
                while (reader.Read())
                {
                    id_tarif.Add(reader.GetString("id_tarif"));
                    comboBox1.Items.Add(reader.GetString("nama_tarif") + " (Rp. " + format_idr(reader.GetString("nominal")) + ")");
                }
                conn.Close();

                SQL = "SELECT id_kamar FROM reservasi WHERE status_out=0 ORDER BY id_kamar ASC;";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString("id_kamar"));
                }
                conn.Close();

                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void isi_combo_box2()
        {
            try
            {
                string id_kmr, id_trf;
                string SQL = "SELECT id_kamar, id_tarif, tgl_sewa, tgl_berhenti FROM extra_bed, reservasi WHERE extra_bed.id_reservasi=reservasi.id_reservasi AND id_extra_bed='" + keterangan + "';";

                id_kmr = "";
                id_trf = "";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_kmr = reader.GetString("id_kamar");
                    id_trf = reader.GetString("id_tarif");
                    dateTimePicker1.Value = reader.GetDateTime("tgl_sewa");
                    dateTimePicker2.Value = reader.GetDateTime("tgl_berhenti");
                }
                conn.Close();

                SQL = "SELECT id_tarif, nama_tarif, nominal FROM tarif WHERE nama_tarif LIKE '%bed%' ORDER BY nama_tarif ASC;";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                id_tarif.Clear();
                while (reader.Read())
                {
                    id_tarif.Add(reader.GetString("id_tarif"));
                    comboBox1.Items.Add(reader.GetString("nama_tarif") + " (Rp. " + format_idr(reader.GetString("nominal")) + ")");
                    if (id_trf == reader.GetString("id_tarif"))
                    {
                        comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                    }
                }
                conn.Close();

                SQL = "SELECT id_kamar FROM kamar ORDER BY id_kamar ASC;";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString("id_kamar"));
                    if (id_kmr == reader.GetString("id_kamar"))
                    {
                        comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime awal, akhir;
            int jumlah_hari;
            awal = dateTimePicker1.Value;
            akhir = dateTimePicker2.Value;
            jumlah_hari = Convert.ToInt32(((akhir - awal).TotalDays));

            if(jumlah_hari < 1)
            {
                MessageBox.Show("Rentang tanggal extra bed minimal 1 hari!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Anda belum memasukkan semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                if (keterangan == "tambah")
                {
                    tambah_data();
                }
                else
                {
                    edit_data();
                }
            }        
        }

        private void tambah_data()
        {
            try
            {
                string no_kmr, id_trf, tgl_sewa, tgl_berhenti;
                no_kmr = comboBox2.SelectedItem.ToString();
                id_trf = id_tarif[comboBox1.SelectedIndex];
                tgl_sewa = dateTimePicker1.Value.ToString("yyyy-M-d");
                tgl_berhenti = dateTimePicker2.Value.ToString("yyyy-M-d");

                string SQL = "INSERT INTO extra_bed (id_reservasi, id_tarif, tgl_sewa, tgl_berhenti, status_selesai) VALUES ('" + cari_id_reservasi(no_kmr) + "','" + id_trf + "','" + tgl_sewa + "','" + tgl_berhenti + "', 0);";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                conn.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private string cari_id_reservasi(string id_kmr)
        {
            string hasil = "";
            try
            {
                string SQL = "SELECT id_reservasi FROM reservasi WHERE status_out=0 AND id_kamar='" + id_kmr + "' ORDER BY id_reservasi DESC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hasil = reader.GetString("id_reservasi");
                    break;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            return hasil;
        }

        private void edit_data()
        {
            try
            {
                string id_trf, tgl_sewa, tgl_berhenti;

                id_trf = id_tarif[comboBox1.SelectedIndex];
                tgl_sewa = dateTimePicker1.Value.ToString("yyyy-M-d");
                tgl_berhenti = dateTimePicker2.Value.ToString("yyyy-M-d");

                string SQL = "UPDATE extra_bed SET id_tarif='" + id_trf + "', tgl_sewa='" + tgl_sewa + "', tgl_berhenti='" + tgl_berhenti + "' WHERE id_extra_bed='" + keterangan + "';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                conn.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
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
      
    }
}

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

        public check_in()
        {
            InitializeComponent();
            isi_kamar(); 
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

                string SQL = "SELECT COUNT(*) FROM tamu WHERE id='"+id+"'";
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
            try
            {
                string id, no_telp, nama, alamat, nomor_kamar;
                id = textBox2.Text;
                id = id.Trim();
                no_telp = textBox4.Text;
                no_telp = no_telp.Trim();
                nama = textBox3.Text;
                nama = nama.Trim();
                alamat = textBox5.Text;
                alamat = alamat.Trim();

                nomor_kamar = comboBox1.SelectedItem.ToString();

                if (id == "" || no_telp == "" || nama == "" || alamat == "")
                {
                    MessageBox.Show("Form masukan belum diisi !", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (cek_tamu_sudah_ada_di_db() == true)
                    {
                        //MessageBox.Show("Berhasil check in!\nNomor kamar " + nomor_kamar, "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string SQL = "INSERT INTO tamu (id, nama, alamat, telepon) VALUES ('" + id + "','" + nama + "','" + alamat + "','" + no_telp + "');";
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(SQL, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                        }
                        conn.Close();
                    }
                    insert_reservasi();
                    insert_trx();
                    MessageBox.Show("Berhasil check in!\nNomor kamar " + nomor_kamar, "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBox1.SelectedIndex = 0;
                    hapus_form();
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void insert_reservasi()
        {
            try
            {
                string id_kamar, tgl, jam;
                id_kamar = comboBox1.SelectedItem.ToString();
                tgl = DateTime.Now.ToString("yyyy-M-d");
                jam = DateTime.Now.ToString("H:m:s");
                string SQL = "INSERT INTO reservasi (id_kamar, tgl_check_in, jam_check_in) VALUES ('" + id_kamar + "','" + tgl + "','" + jam + "');";
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

        private void insert_trx()
        {
            try
            {
                string id_reservasi, id_tamu;
                string SQL = "SELECT MAX(id) FROM reservasi;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                id_reservasi = Convert.ToString(cmd.ExecuteScalar());
                conn.Close();

                id_tamu = textBox2.Text;
                id_tamu = id_tamu.Trim();
                SQL = "INSERT INTO transaksi (id_reservasi, id_tamu) VALUES ('" + id_reservasi + "','" + id_tamu + "');";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
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
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nama_kamar;
            nama_kamar = comboBox1.SelectedItem.ToString();
            if(apakah_penuh(nama_kamar)==true)
            {
                label5.Text = "Kamar sudah terisi!";
                label5.ForeColor = Color.Red;
                button2.Enabled = true;
            }
            else
            {
                label5.Text = "Kamar masih kosong!";
                label5.ForeColor = Color.Green;
                button2.Enabled = false;
            }
        }

        private bool apakah_penuh(string kamar)
        {
            try
            {             
                string SQL = "SELECT COUNT(*) FROM reservasi WHERE id_kamar='" + kamar + "'";
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

        private void button2_Click(object sender, EventArgs e)
        {
            //((detail_tamu_satu_kamar)f).id_kamar = int.Parse(comboBox1.SelectedItem.ToString());
            detail_tamu_satu_kamar a = new detail_tamu_satu_kamar(int.Parse(comboBox1.SelectedItem.ToString()));
            DialogResult dr = a.ShowDialog();
        }
    }
}

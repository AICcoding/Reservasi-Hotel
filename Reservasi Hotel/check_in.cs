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
        MySqlConnection conn = conectionservice.getconection();
        public check_in()
        {
            InitializeComponent();
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
                string id, no_telp, nama, alamat;
                id = textBox2.Text;
                id = id.Trim();
                no_telp = textBox4.Text;
                no_telp = no_telp.Trim();
                nama = textBox3.Text;
                nama = nama.Trim();
                alamat = textBox5.Text;
                alamat = alamat.Trim();

                if (id == "" || no_telp == "" || nama == "" || alamat == "")
                {
                    MessageBox.Show("Form masukan belum diisi !", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (cek_tamu_sudah_ada_di_db() == true)
                    {
                        MessageBox.Show("Berhasil check in!\nNOmor kamar  xxx", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Berhasil check in!\nNOmor kamar  xxx", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    hapus_form();
                }             
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
    }
}

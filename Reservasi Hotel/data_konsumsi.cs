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
    public partial class data_konsumsi : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        string keterangan, hasil_nominal;

        public data_konsumsi(string ket)
        {
            InitializeComponent();
            keterangan = ket;
            init();
        }

        private void init()
        {
            if(keterangan=="tambah")
            {
                this.Text = "Tambah konsumsi";
                label1.Text = "TAMBAH KONSUMSI";
                toolTip1.SetToolTip(button1, "Tambah konsumsi");
                
            }
            else
            {
                this.Text = "Edit konsumsi";
                label1.Text = "EDIT KONSUMSI";
                toolTip1.SetToolTip(button1, "Edit konsumsi");
                isi_data_awal();
            }
        }

        private void isi_data_awal()
        {
            try
            {
                string SQL = "SELECT *  FROM konsumsi WHERE id_konsumsi='"+keterangan+"';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox2.Text = reader.GetString("nama");
                    textBox1.Text = reader.GetString("harga");
                    format_idr();
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
            if (textBox2.Text.Trim() != "" && textBox1.Text.Trim() != "")
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
            else
            {
                MessageBox.Show("Anda belum memasukkan semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void tambah_data()
        {
            try
            {
                string nama, nominal;
                nama = textBox2.Text.Trim();
                nominal = textBox1.Text.Trim();
                nominal = nominal.Replace(".", "");

                string SQL = "INSERT INTO konsumsi (nama, harga) VALUES ('"+nama+"','"+nominal+"');";
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

        private void edit_data()
        {
            try
            {
                string nama, nominal;
                int id_konsumsi = int.Parse(keterangan);
                nama = textBox2.Text.Trim();
                nominal = textBox1.Text.Trim();
                nominal = nominal.Replace(".", "");

                string SQL = "UPDATE konsumsi SET nama='" + nama + "', harga='" + nominal + "' WHERE id_konsumsi='" + id_konsumsi + "';";
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                format_idr(e.KeyChar);                
            }
        }

        private void format_idr(char input = 'a')
        {
            if (input != '\b' && input!='a')
            {
                int hitung;
                string tmp;
                char[] tmp_input;

                tmp = textBox1.Text;
                tmp = tmp.Replace(".", "");
                hasil_nominal = "";
                hitung = 2 - ((tmp.Length + 1) % 3);
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
                textBox1.Text = hasil_nominal;
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else if (input == '\b')
            {
                int hitung;
                string tmp;
                char[] tmp_input;

                tmp = textBox1.Text;
                tmp = tmp.Replace(".", "");
                hasil_nominal = "";
                hitung = 2 - ((tmp.Length - 1) % 3);
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
                try
                {
                    hasil_nominal = hasil_nominal.Remove(hasil_nominal.Length - 1);
                }
                catch (Exception e) { }
                textBox1.Text = hasil_nominal;
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else
            {
                int hitung;
                string tmp;
                char[] tmp_input;

                tmp = textBox1.Text;
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
                textBox1.Text = hasil_nominal;
                textBox2.Focus();
            }
            
        }   
      
    }
}

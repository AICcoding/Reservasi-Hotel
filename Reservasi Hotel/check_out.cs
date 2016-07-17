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
    public partial class check_out : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];
        List<string> id_tamu;
        bool sudah_bisa_bayar;



        public check_out()
        {
            InitializeComponent();
            this.Width = ((Form1)f).panel2.Width;
            this.Height = ((Form1)f).treeView1.Height-50;
            comboBox1.SelectedIndex = 1;
            radioButton1.Checked = true;
            comboBox1.Enabled = false;
            textBox10.Enabled = false;
            load_kamar();
            update_form_pembayaran();
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }           
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                comboBox1.Enabled = false;
                textBox10.Enabled = false;
                textBox10.Text = "";
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.Enabled = true;
                textBox10.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                comboBox1.Enabled = true;
                textBox10.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
                textBox10.Enabled = false;
                textBox10.Text = "";
                comboBox1.SelectedIndex = 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                sudah_bisa_bayar = false;
                update_form_pembayaran();
                kosongkan_detail_tamu();

                string id_reservasi = "";
                string no_kamar = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string SQL = "SELECT id_reservasi FROM reservasi WHERE id_kamar='" + no_kamar + "' AND status_out=0;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_reservasi = reader.GetString("id_reservasi");
                }
                conn.Close();

                SQL = "SELECT * FROM transaksi_tamu WHERE id_reservasi='" + id_reservasi + "';";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                int indek = -1;
                id_tamu = new List<string>();
                dataGridView2.Rows.Clear();
                while (reader.Read())
                {
                    id_tamu.Add(reader.GetString("id_tamu"));
                    dataGridView2.Rows.Add();
                    indek += 1;
                    dataGridView2.Rows[indek].Cells[0].Value = reader.GetString("nama");
                    dataGridView2.Rows[indek].Cells[1].Value = reader.GetDateTime("tgl_masuk").ToString("dd-MM-yyyy");
                    dataGridView2.Rows[indek].Cells[2].Value = reader.GetDateTime("tgl_keluar").ToString("dd-MM-yyyy");
                    dataGridView2.Rows[indek].Cells[3].Value = "sng tawang...";
                    dataGridView2.Rows[indek].Cells[4].Value = format_idr(reader.GetString("jumlah_bayar"));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string id_tamu_tmp = id_tamu[dataGridView2.CurrentCell.RowIndex];
                string SQL = "SELECT nama, alamat, telepon, tgl_masuk, tgl_keluar FROM transaksi_tamu WHERE id_tamu='" + id_tamu_tmp + "';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox3.Text = reader.GetString("nama");
                    textBox4.Text = reader.GetString("telepon");
                    textBox5.Text = reader.GetString("alamat");
                    textBox1.Text = convert_tgl_indonesia(reader.GetDateTime("tgl_masuk").ToString("yyyy-MM-dd"));
                    textBox2.Text = convert_tgl_indonesia(reader.GetDateTime("tgl_keluar").ToString("yyyy-MM-dd"));
                }
                conn.Close();

                sudah_bisa_bayar = true;
                update_form_pembayaran();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox7.Text = format_idr(textBox7.Text);
            textBox7.SelectionStart = textBox7.Text.Length;
            update_total_harga();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox8.Text = format_idr(textBox8.Text);
            textBox8.SelectionStart = textBox8.Text.Length;
            update_total_harga();
        }




        private void load_kamar()
        {
            try
            {
                List<string> id_kamar = new List<string>();
                string SQL = "SELECT id_kamar, lantai FROM kamar ORDER BY id_kamar ASC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_kamar.Add(reader.GetString("id_kamar"));
                }
                conn.Close();

                int indek = -1;
                dataGridView1.Rows.Clear();
                for (int i = 0; i < id_kamar.Count; i++)
                {
                    if(cek_kamar_sudah_terisi(id_kamar[i])==true)
                    {
                        dataGridView1.Rows.Add();
                        indek += 1;
                        dataGridView1.Rows[indek].Cells[0].Value = id_kamar[i];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
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

        private void update_form_pembayaran()
        {
            if(sudah_bisa_bayar==true)
            {
                groupBox1.Enabled = true;
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                radioButton1.Checked = true;
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                groupBox1.Enabled = false;
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                radioButton1.Checked = true;
                comboBox1.SelectedIndex = 0;
            }
        }

        private void kosongkan_detail_tamu()
        {
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void update_total_harga()
        {
            int h1, h2;
            string tmp;
            try
            {
                tmp = textBox7.Text.Replace(".", "");
                h1 = Convert.ToInt32(tmp);
            }
            catch(Exception e)
            {
                h1 = 0;
            }

            try
            {
                tmp = textBox8.Text.Replace(".", "");
                h2 = Convert.ToInt32(tmp);
            }
            catch (Exception e)
            {
                h2 = 0;
            }

            textBox9.Text = format_idr((h1 + h2).ToString());
        }

        private string convert_tgl_indonesia(string yyyy_mm_dd)
        {
            string[] tmp_tgl = yyyy_mm_dd.Split('-');
            string hasil = tmp_tgl[2];
            switch(Convert.ToInt32(tmp_tgl[1]))
            {
                case 1:
                    hasil += " Januari";
                    break;
                case 2:
                    hasil += " Februari";
                    break;
                case 3:
                    hasil += " Maret";
                    break;
                case 4:
                    hasil += " April";
                    break;
                case 5:
                    hasil += " Mei";
                    break;
                case 6:
                    hasil += " Juni";
                    break;
                case 7:
                    hasil += " Juli";
                    break;
                case 8:
                    hasil += " Agustus";
                    break;
                case 9:
                    hasil += " September";
                    break;
                case 10:
                    hasil += " Oktober";
                    break;
                case 11:
                    hasil += " November";
                    break;
                case 12:
                    hasil += " Desember";
                    break;
            }
            hasil += " " + tmp_tgl[0];
            return hasil;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("konden ade ne brow...");
        }

         

    }
}

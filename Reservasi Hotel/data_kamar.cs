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
    public partial class data_kamar : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        string keterangan;
        List<string> id_tarif;

        public data_kamar(string ket)
        {
            InitializeComponent();
            keterangan = ket;
            id_tarif = new List<string>();
            init();
        }

        private void init()
        {
            sudah_ada_id_kamar(1);
            try
            {
                string SQL = "SELECT id_tarif, nama_tarif, nominal FROM tarif ORDER BY nama_tarif ASC;";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

            if(keterangan=="tambah")
            {
                this.Text = "Tambah kamar";
                label1.Text = "TAMBAH KAMAR";
                toolTip1.SetToolTip(button1, "Tambah kamar");
                
            }
            else
            {
                this.Text = "Edit kamar";
                label1.Text = "EDIT KAMAR";
                textBox2.ReadOnly = true;
                toolTip1.SetToolTip(button1, "Edit kamar");
                isi_data_awal();
            }          
        }

        private void isi_data_awal()
        {
            try
            {
                int indek_tarif=0;
                string id_trf;

                string SQL = "SELECT *  FROM kamar WHERE id_kamar='"+keterangan+"';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    textBox2.Text = reader.GetString("id_kamar");
                    id_trf = reader.GetString("id_tarif");
                    for(int i=0;i<id_tarif.Count;i++)
                    {
                        if(id_tarif[i]==id_trf)
                        {
                            indek_tarif=i;
                            break;
                        }
                    }
                    comboBox1.SelectedIndex = indek_tarif;
                    textBox1.Text = reader.GetString("fasilitas");
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
            if (textBox2.Text.Trim() != "" && textBox1.Text.Trim() != "" && comboBox1.SelectedIndex != -1)
            {
                if (keterangan == "tambah")
                {
                    if (sudah_ada_id_kamar(int.Parse(textBox2.Text.Trim())))
                    {
                        MessageBox.Show("Nomor kamar " + textBox2.Text.Trim() + " sudah ada!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if(int.Parse(textBox2.Text.Trim()) < 1)
                    {
                        MessageBox.Show("Nomor kamar yang dimasukkan tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        tambah_data();
                    }      
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
                string no_kamar, id_trf, fasilitas;
                no_kamar = textBox2.Text.Trim();
                id_trf = id_tarif[comboBox1.SelectedIndex];
                fasilitas = textBox1.Text.Trim();

                string SQL = "INSERT INTO kamar (id_kamar, id_tarif, fasilitas) VALUES ('" + no_kamar + "','" + id_trf + "','" + fasilitas + "');";
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
                string no_kamar, id_trf, fasilitas;
                no_kamar = textBox2.Text.Trim();
                id_trf = id_tarif[comboBox1.SelectedIndex];
                fasilitas = textBox1.Text.Trim();

                string SQL = "UPDATE kamar SET id_kamar='" + no_kamar + "', id_tarif='" + id_trf + "', fasilitas='"+fasilitas+"' WHERE id_kamar='" + keterangan + "';";
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

        private bool sudah_ada_id_kamar(int id_kmr)
        {
            try
            {
                int jumlah_baris;
                string SQL = "SELECT COUNT(*) FROM kamar WHERE id_kamar = " + id_kmr + ";";
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
      
    }
}

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
    public partial class pilih_kamar : Form
    {
        MySqlConnection conn = conectionservice.getconection();

        public pilih_kamar()
        {
            InitializeComponent();
            load_kamar();
        }

        private void load_kamar()
        {
            try
            {
                dataGridView1.Rows.Clear();

                string SQL = "SELECT id_kamar, fasilitas, nama_tarif, nominal FROM kamar, tarif WHERE kamar.id_tarif=tarif.id_tarif;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i = -1;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    i += 1;
                    dataGridView1.Rows[i].Cells[0].Value = reader.GetString("id_kamar");
                    dataGridView1.Rows[i].Cells[1].Value = reader.GetString("fasilitas");
                    dataGridView1.Rows[i].Cells[2].Value = format_idr(reader.GetString("nominal")) +" ("+reader.GetString("nama_tarif")+")";
                }
                conn.Close();
                cek_status();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void cek_status()
        {
            try
            {
                int indek_data;
                int[] data_kamar;
                data_kamar = new int[1];
                string SQL = "SELECT MAX(id_kamar) FROM kamar;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data_kamar = new int[int.Parse(reader.GetString("MAX(id_kamar)"))];
                }
                conn.Close();

                for (int i = 0; i < data_kamar.Length; i++)
                {
                    data_kamar[i] = 0;
                }

                SQL = "SELECT nama, id_kamar FROM transaksi, tamu, reservasi WHERE transaksi.id_tamu=tamu.id_tamu AND reservasi.id_reservasi=transaksi.id_reservasi AND status_out=0 AND tgl_keluar is null;";
                conn.Open();
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data_kamar[int.Parse(reader.GetString("id_kamar")) - 1] += 1;
                }
                conn.Close();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    indek_data = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    if (data_kamar[indek_data - 1]==0)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "Kosong";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "Ada "+data_kamar[indek_data - 1]+" orang";
                    }
                }
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
            this.ReturnValue1 = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string ReturnValue1 { get; set; }

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

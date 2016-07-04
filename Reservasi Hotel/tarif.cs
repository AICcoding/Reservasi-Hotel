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
    public partial class tarif : Form
    {
        MySqlConnection conn = conectionservice.getconection();

        public tarif()
        {
            InitializeComponent();
            load_tarif();
        }

        private void load_tarif()
        {
            try
            {
                dataGridView1.Rows.Clear();
             
                string SQL = "SELECT * FROM tarif;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i=-1;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    i+=1;
                    dataGridView1.Rows[i].Cells[0].Value = reader.GetString("id_tarif");
                    dataGridView1.Rows[i].Cells[1].Value = reader.GetString("nama_tarif");
                    dataGridView1.Rows[i].Cells[2].Value = format_idr(reader.GetString("nominal"));          
                }
                conn.Close();
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
      

        private void button1_Click(object sender, EventArgs e)
        {
            data_tarif a = new data_tarif("tambah");
            DialogResult dr = a.ShowDialog();
            load_tarif();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Apakah anda yakin ingin menghapus tarif terpilih?", "Hapus tarif", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                hapus_tarif_terpilih();
                load_tarif();
            }
        }

        private void hapus_tarif_terpilih()
        {
            try
            {
                int indek_tarif = int.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
                string SQL = "DELETE FROM tarif WHERE id_tarif=" + indek_tarif+";";
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

        private void button2_Click(object sender, EventArgs e)
        {
            string indek_tarif = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            data_tarif a = new data_tarif(indek_tarif);
            DialogResult dr = a.ShowDialog();
            load_tarif();
        }
    }
}

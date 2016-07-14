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
    public partial class cari_no_kamar : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        public cari_no_kamar()
        {
            InitializeComponent();
            load_kamar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string no_kamar = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                if (cek_kamar_sudah_terisi(no_kamar) == true)
                {
                    detail_kamar a = new detail_kamar();
                    a.nomorKamar = Convert.ToInt32(no_kamar);
                    DialogResult dr = a.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Kamar " + no_kamar + " Kosong!", "Gagal check out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception er) { }            
        } 

        private void load_kamar()
        {
            try
            {
                string SQL = "SELECT id_kamar, lantai FROM kamar ORDER BY id_kamar ASC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i = -1;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    i+=1;
                    dataGridView1.Rows[i].Cells[0].Value = reader.GetString("id_kamar");
                    dataGridView1.Rows[i].Cells[1].Value = reader.GetString("lantai");
                }
                conn.Close();               
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
    }
}

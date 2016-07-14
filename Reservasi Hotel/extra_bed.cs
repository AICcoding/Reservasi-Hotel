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
    public partial class extra_bed : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        List<string> id_extra_bed;

        public extra_bed()
        {
            InitializeComponent();
            id_extra_bed = new List<string>();
            load_extra_bed();
        }

        private void load_extra_bed()
        {
            try
            {
                DateTime tgl_awal, tgl_akhir, tgl_sekarang;
                int sisa_hari;
                tgl_sekarang = DateTime.Now;
                dataGridView1.Rows.Clear();

                string SQL = "SELECT id_extra_bed, id_kamar, tgl_sewa, tgl_berhenti FROM extra_bed, reservasi WHERE extra_bed.id_reservasi=reservasi.id_reservasi AND status_selesai=0 ORDER BY tgl_berhenti ASC;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i=-1;
                id_extra_bed.Clear();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    i+=1;
                    id_extra_bed.Add(reader.GetString("id_extra_bed"));
                    dataGridView1.Rows[i].Cells[0].Value = reader.GetString("id_kamar");
                    dataGridView1.Rows[i].Cells[1].Value = konversi_tgl(reader.GetDateTime("tgl_sewa").ToString("yyyy-M-d"));
                    dataGridView1.Rows[i].Cells[2].Value = konversi_tgl(reader.GetDateTime("tgl_berhenti").ToString("yyyy-M-d"));
                    tgl_awal = reader.GetDateTime("tgl_sewa");
                    tgl_akhir = reader.GetDateTime("tgl_berhenti");
                    sisa_hari = Convert.ToInt32((tgl_akhir - tgl_awal).TotalDays);
                    if(sisa_hari < 1)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "Extra bed expired";
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if(sisa_hari==1)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "1 hari tersisa";
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[3].Value = sisa_hari+" hari tersisa";
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

        private string konversi_tgl(string tgl)
        {
            string[] tmp_tgl = tgl.Split('-');
            string hasil_tgl, hasil;
            hasil_tgl = tmp_tgl[2];
            int bulan = Convert.ToInt32(tmp_tgl[1]);
            switch (bulan)
            {
                case 1:
                    hasil_tgl += " Januari ";
                    break;
                case 2:
                    hasil_tgl += " Februari ";
                    break;
                case 3:
                    hasil_tgl += " Maret ";
                    break;
                case 4:
                    hasil_tgl += " April ";
                    break;
                case 5:
                    hasil_tgl += " Mei ";
                    break;
                case 6:
                    hasil_tgl += " Juni ";
                    break;
                case 7:
                    hasil_tgl += " Juli ";
                    break;
                case 8:
                    hasil_tgl += " Agustus ";
                    break;
                case 9:
                    hasil_tgl += " September ";
                    break;
                case 10:
                    hasil_tgl += " Oktober ";
                    break;
                case 11:
                    hasil_tgl += " November ";
                    break;
                case 12:
                    hasil_tgl += " Desember ";
                    break;
            }
            hasil_tgl += tmp_tgl[0];
            hasil = hasil_tgl;
            return hasil;
        }     

        private void button1_Click(object sender, EventArgs e)
        {
            data_extra_bed a = new data_extra_bed("tambah");
            DialogResult dr = a.ShowDialog();
            load_extra_bed();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int tes_error = dataGridView1.CurrentCell.RowIndex;
                DialogResult dialogResult = MessageBox.Show("Apakah anda yakin ingin menghapus extra bed terpilih?", "Hapus extra bed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    hapus_extra_bed_terpilih();
                    load_extra_bed();
                }
            }
            catch (Exception er) { }
        }

        private void hapus_extra_bed_terpilih()
        {
            try
            {
                int indek_extra_bed = int.Parse(id_extra_bed[dataGridView1.CurrentCell.RowIndex]);
                string SQL = "DELETE FROM extra_bed WHERE id_extra_bed=" + indek_extra_bed+";";
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
            try
            {
                string indek_extra_bed = id_extra_bed[dataGridView1.CurrentCell.RowIndex];
                data_extra_bed a = new data_extra_bed(indek_extra_bed);
                DialogResult dr = a.ShowDialog();
                load_extra_bed();
            }
            catch (Exception er) { }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Apakah anda yakin layanan extra bed pada kamar " + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString() + " sudah selesai?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    update_status_extra_bed_terpilih();
                    load_extra_bed();
                }
            }
            catch (Exception er) { }         
        }

        private void update_status_extra_bed_terpilih()
        {
            try
            {
                int indek_extra_bed = int.Parse(id_extra_bed[dataGridView1.CurrentCell.RowIndex]);
                string tgl_sekarang = DateTime.Now.ToString("yyyy-M-d");
                string SQL = "UPDATE extra_bed  SET status_selesai=1, tgl_berhenti='"+tgl_sekarang+"' WHERE id_extra_bed=" + indek_extra_bed + ";";
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
    }
}

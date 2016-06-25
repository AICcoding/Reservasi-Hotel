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
    public partial class detail_tamu_satu_kamar : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        public int id_kamar;
        public detail_tamu_satu_kamar(int kamar)
        {
            InitializeComponent();
            id_kamar = kamar;
            isi_data_grid_view();
        } 
    
        private void isi_data_grid_view()
        {
            try
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT tamu.nama, tamu.alamat, tamu.telepon FROM transaksi, tamu WHERE transaksi.id_tamu=tamu.id AND tgl_check_out IS NULL;";
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds, "hasil");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "hasil";
                conn.Close();

                dataGridView1.Columns[0].HeaderText = "Nama tamu";
                dataGridView1.Columns[1].HeaderText = "Alamat";
                dataGridView1.Columns[2].HeaderText = "Nomor telp.";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

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

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
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
            }
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
                for (int i = 0; i < id_kamar.Count; i++)
                {
                    if(cek_kamar_sudah_terisi(id_kamar[i])==false)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}

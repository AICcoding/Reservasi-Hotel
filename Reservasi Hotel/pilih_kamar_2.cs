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
    public partial class pilih_kamar_2 : Form
    {
        int jlh_kmr,booked;
        Button[] kmr;
        string[] id_kamar;
        int select = -1;


        void set_booked()
        {
            for(int i=0; i<booked; i++)
            {
                kmr[i].BackColor = Color.Red;
            }
        }

        void buat_view()
        {
            kmr = new Button[jlh_kmr];
            int st_y = 34;
            for (int i = 0; i < jlh_kmr; i++)
            {
                if (i > 3)
                {
                    if (i % 5 == 0)
                    {
                        st_y += 101;
                    }
                }
                kmr[i] = new System.Windows.Forms.Button();
                this.panel1.Controls.Add(kmr[i]);
                kmr[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (i % 5 == 0)
                {
                    kmr[i].Location = new System.Drawing.Point(49, st_y);
                }
                else if (i % 5 == 1)
                {
                    kmr[i].Location = new System.Drawing.Point(173, st_y);
                }
                else if (i % 5 == 2)
                {
                    kmr[i].Location = new System.Drawing.Point(297, st_y);
                }
                else if (i % 5 == 3)
                {
                    kmr[i].Location = new System.Drawing.Point(421, st_y);
                }
                else
                {
                    kmr[i].Location = new System.Drawing.Point(545, st_y);
                }

                kmr[i].Name = "btn" + (i + 1);
                kmr[i].Size = new System.Drawing.Size(75, 67);
                kmr[i].TabIndex = 2;
                kmr[i].Text = id_kamar[i];
                kmr[i].UseVisualStyleBackColor = true;
                kmr[i].BackColor = Color.LightGreen;
                kmr[i].Click += new System.EventHandler(this.button_Click);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            if(clicked.BackColor == Color.LightSkyBlue)
            {
                clicked.BackColor = Color.LightGreen;
                select = -1;
            }
            else if (clicked.BackColor == Color.LightGreen)
            {
                clicked.BackColor = Color.LightSkyBlue;
                for(int i=0; i < kmr.Length; i++)
                {
                    if(kmr[i].Text == select.ToString())
                    {
                        kmr[i].BackColor = Color.LightGreen;
                    }
                }
                select = Convert.ToInt32(clicked.Text);
            }
            //clicked.BackColor = Color.Red;
            //this.BackColor = Color.Red;
            //MessageBox.Show();
        }

        public pilih_kamar_2()
        {
            InitializeComponent();
            MySqlConnection conn = conectionservice.getconection();
            using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT type FROM kamar", conn))
            {
                conn.Open();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetString(0));
                    /*tgl_check_in = dataReader.GetDateTime(2);
                    jam_check_in = Convert.ToDateTime(dataReader.GetString(3));
                    totalTerbayar = dataReader.GetString(4);*/
                }
                conn.Close();
            }
            using (MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT lantai FROM kamar", conn))
            {
                conn.Open();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString(0));
                    /*tgl_check_in = dataReader.GetDateTime(2);
                    jam_check_in = Convert.ToDateTime(dataReader.GetString(3));
                    totalTerbayar = dataReader.GetString(4);*/
                }
                conn.Close();
            }
        }

        private void pilih_kamar_2_Load(object sender, EventArgs e)
        {
            jlh_kmr = 0;
            booked = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection conn = conectionservice.getconection();
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM kamar WHERE lantai=" + comboBox1.Text + " AND type='" + comboBox2.Text + "'", conn))
                {
                    conn.Open();
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        jlh_kmr = dataReader.GetInt32(0);
                    }
                    conn.Close();
                }
                using (MySqlCommand cmd = new MySqlCommand("SELECT id_kamar FROM kamar WHERE lantai=" + comboBox1.Text + " AND type='" + comboBox2.Text + "'", conn))
                {
                    conn.Open();
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    int i = 0;
                    id_kamar = new string[jlh_kmr];
                    while (dataReader.Read())
                    {
                        id_kamar[i] = dataReader.GetString(0);
                        i++;
                    }
                    conn.Close();
                }
                if (kmr != null)
                {
                    for (int i = 0; i < kmr.Length; i++)
                    {
                        if (kmr[i] == null)
                            break;

                        kmr[i].Click -= new EventHandler(this.button_Click);
                        this.panel1.Controls.Remove(kmr[i]);
                        kmr[i].Dispose();
                    }
                }
                buat_view();
                set_booked();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string ReturnValue1 { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            if(select == -1)
            {
                MessageBox.Show("Silahkan Pilih Kamar !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                this.ReturnValue1 = select.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

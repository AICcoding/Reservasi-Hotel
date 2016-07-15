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
    public partial class booking : Form
    {
        MySqlConnection conn = conectionservice.getconection();
        public booking()
        {
            InitializeComponent();
        }

        int get_month(int bln)
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            int yy = datevalue.Year;
            if(bln < 8)
            {
                if(bln == 2)
                {
                    if(yy%4 == 0)
                    {
                        return 29;
                    }
                    else
                    {
                        return 28;
                    }
                }
                else if(bln%2 == 0)
                {
                    return 30;
                }
                else
                {
                    return 31;
                }
            }
            else
            {
                if(bln%2 == 0)
                {
                    return 31;
                }
                else
                {
                    return 30;
                }
            }
        }

        void set_data(int mm, int yy)
        {
            dataGridView1.ColumnCount = get_month(mm) + 1;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderText = i.ToString();
            }
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 25;
                for (int j = 1; j < dataGridView1.RowCount; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Yellow;
                }
            }
            String SQL = "SELECT id_kamar FROM kamar";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int counter = 0;
            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[counter].Cells[0].Value = reader.GetString("id_kamar");
                counter++;
            }
            conn.Close();


            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                conn.Open();
                SQL = "SELECT id_kamar FROM reservasi WHERE tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in <= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "')";
                //SQL = "SELECT id_kamar FROM reservasi WHERE tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in < '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "')";
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                counter = 0;
                while (reader.Read())
                {
                    for (int x = 0; x < dataGridView1.RowCount - 1; x++)
                    {
                        if (dataGridView1.Rows[x].Cells[0].Value.ToString() == reader.GetString("id_kamar"))
                        {
                            dataGridView1.Rows[x].Cells[i].Style.BackColor = Color.Red;
                        }
                    }
                    counter++;
                }
                conn.Close();
            }
        }

        private void booking_Load(object sender, EventArgs e)
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            int mm = datevalue.Month;
            int yy = datevalue.Year;
            set_data(mm, yy);
            textBox1.Text = yy.ToString();
            textBox2.Text = mm.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int temp1, temp2;
            temp1 = Convert.ToInt32(textBox1.Text);
            temp2 = Convert.ToInt32(textBox2.Text);
            if(temp2 == 1)
            {
                temp2 = 12;
                temp1--;
            }
            else
            {
                temp2--;
            }
            textBox1.Text = temp1.ToString();
            textBox2.Text = temp2.ToString();
            dataGridView1.Rows.Clear();
            set_data(temp2, temp1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int temp1, temp2;
            temp1 = Convert.ToInt32(textBox1.Text);
            temp2 = Convert.ToInt32(textBox2.Text);
            if (temp2 == 12)
            {
                temp2 = 1;
                temp1++;
            }
            else
            {
                temp2++;
            }
            textBox1.Text = temp1.ToString();
            textBox2.Text = temp2.ToString();
            dataGridView1.Rows.Clear();
            set_data(temp2, temp1);
        }
    }
}

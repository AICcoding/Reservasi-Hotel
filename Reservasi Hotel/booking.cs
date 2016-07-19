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
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];

        public booking()
        {
            InitializeComponent();
            this.Width = ((Form1)f).panel2.Width;
            this.Height = ((Form1)f).treeView1.Height - 50;

            label1.Visible = false;
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
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
            dataGridView1.ColumnCount = get_month(mm);
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderText = (i+1).ToString();
            }

            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 1; j < dataGridView1.RowCount; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Yellow;
                }
            }

            String SQL = "SELECT id_kamar FROM kamar ORDER BY id_kamar ASC;";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int counter = 0;
            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[counter].HeaderCell.Value = reader.GetString("id_kamar");
                counter++;
            }
            conn.Close();

            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                conn.Open();
                SQL = "SELECT id_kamar FROM reservasi WHERE status_out=0 AND (tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in <= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "'))";
                //SQL = "SELECT id_kamar FROM reservasi WHERE tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in < '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "')";
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                counter = 0;
                while (reader.Read())
                {
                    for (int x = 0; x < dataGridView1.RowCount; x++)
                    {
                        if (dataGridView1.Rows[x].HeaderCell.Value.ToString() == reader.GetString("id_kamar"))
                        {
                            dataGridView1.Rows[x].Cells[i-1].Style.BackColor = Color.Red;
                        }
                    }
                    counter++;
                }
                conn.Close();
            }
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                conn.Open();
                SQL = "SELECT id_kamar FROM reservasi WHERE status_out=-1 AND (tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in <= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "'))";
                //SQL = "SELECT id_kamar FROM reservasi WHERE tgl_check_in = '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' OR ( tgl_check_in < '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "' AND tgl_check_out >= '" + yy.ToString() + "-" + mm.ToString() + "-" + i.ToString() + "')";
                cmd = new MySqlCommand(SQL, conn);
                reader = cmd.ExecuteReader();
                counter = 0;
                while (reader.Read())
                {
                    for (int x = 0; x < dataGridView1.RowCount; x++)
                    {
                        if (dataGridView1.Rows[x].HeaderCell.Value.ToString() == reader.GetString("id_kamar"))
                        {
                            dataGridView1.Rows[x].Cells[i - 1].Style.BackColor = Color.Yellow;
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
            label9.Text = convert_string_bulan(mm) + " " + yy.ToString();
            button1.Enabled = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int temp1, temp2;
            temp1 = Convert.ToInt32(textBox1.Text);
            temp2 = Convert.ToInt32(textBox2.Text);
            if (temp2 == 1)
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
            label9.Text = convert_string_bulan(temp2) + " " + temp1.ToString();
            dataGridView1.Rows.Clear();
            set_data(temp2, temp1);
        }

        private void button3_Click(object sender, EventArgs e)
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
            label9.Text = convert_string_bulan(temp2) + " " + temp1.ToString();
            dataGridView1.Rows.Clear();
            set_data(temp2, temp1);
        }

        private string convert_string_bulan(int bulan)
        {
            string hasil = "";
            switch (bulan)
            {
                case 1:
                    hasil = "Januari";
                    break;
                case 2:
                    hasil = "Februari";
                    break;
                case 3:
                    hasil = "Maret";
                    break;
                case 4:
                    hasil = "April";
                    break;
                case 5:
                    hasil = "Mei";
                    break;
                case 6:
                    hasil = "Juni";
                    break;
                case 7:
                    hasil = "Juli";
                    break;
                case 8:
                    hasil = "Agustus";
                    break;
                case 9:
                    hasil = "September";
                    break;
                case 10:
                    hasil = "Oktober";
                    break;
                case 11:
                    hasil = "November";
                    break;
                case 12:
                    hasil = "Desember";
                    break;
            }
            return hasil;
        }


        int _selectedRow = -1;
        int _selectedColumn = -1;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            switch (dataGridView1.SelectedCells.Count)
            {
                case 0:
                    // store no current selection
                    _selectedRow = -1;
                    _selectedColumn = -1;
                    return;
                case 1:
                    // store starting point for multi-select
                    _selectedRow = dataGridView1.SelectedCells[0].RowIndex;
                    _selectedColumn = dataGridView1.SelectedCells[0].ColumnIndex;
                    try
                    {
                        label7.Text = dataGridView1.Rows[_selectedRow].HeaderCell.Value.ToString();
                        dateTimePicker1.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(dataGridView1.Columns[_selectedColumn].HeaderText));
                        dateTimePicker2.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(dataGridView1.Columns[_selectedColumn].HeaderText));
                        if (dataGridView1.Rows[_selectedRow].Cells[_selectedColumn].Style.BackColor == Color.Red)
                        {
                            label11.ForeColor = Color.Red;
                            label11.Text = "*Maaf, rentang tanggal yang dimasukkan tidak tersedia kamar kosong!";
                            button1.Enabled = false;
                        }
                        else
                        {
                            label11.ForeColor = Color.Green;
                            label11.Text = "*Tersedia kamar kosong untuk rentang tanggal yang dimasukkan!";
                            button1.Enabled = true;
                        }
                    }
                    catch (Exception er) { }

                    return;
            }

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (cell.RowIndex == _selectedRow)
                {
                    if (cell.ColumnIndex != _selectedColumn)
                    {
                        _selectedColumn = -1;
                    }
                }
                /*else if (cell.ColumnIndex == _selectedColumn)
                {
                    if (cell.RowIndex != _selectedRow)
                    {
                        _selectedRow = -1;
                    }
                }*/
                // otherwise the cell selection is illegal - de-select
                else
                {
                    cell.Selected = false;
                }
            }
            update_detail_booking();            
        }

        private void update_detail_booking()
        {
            int x1 = 1, x2 = 1;
            bool awal = true, tersedia = false;
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if(awal==true)
                {
                    x1 = Convert.ToInt32(dataGridView1.Columns[cell.ColumnIndex].HeaderText);
                    awal = false;
                }
                else
                {
                    x2 = Convert.ToInt32(dataGridView1.Columns[cell.ColumnIndex].HeaderText);
                }               
            }
          
            if(x1 > x2)
            {
                dateTimePicker1.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), x2);
                dateTimePicker2.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), x1);
            }
            else
            {
                dateTimePicker1.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), x1);
                dateTimePicker2.Value = new DateTime(int.Parse(textBox1.Text), int.Parse(textBox2.Text), x2);
            }

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (dataGridView1.Rows[_selectedRow].Cells[cell.ColumnIndex].Style.BackColor == Color.Red)
                {
                    tersedia = false;
                    break;
                }
                else
                {
                    tersedia = true;
                }
            }

            if (tersedia == false)
            {
                label11.ForeColor = Color.Red;
                label11.Text = "*Maaf, rentang tanggal yang dimasukkan tidak tersedia kamar kosong!";
                button1.Enabled = false;
            }
            else
            {
                label11.ForeColor = Color.Green;
                label11.Text = "*Tersedia kamar kosong untuk rentang tanggal yang dimasukkan!";
                button1.Enabled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (((dateTimePicker2.Value - dateTimePicker1.Value).TotalDays < 0) || ((dateTimePicker1.Value - DateTime.Now).TotalDays < 0))
            {
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            
            try
            {
                bool ada = false;
                string SQL = "SELECT COUNT(*) FROM reservasi WHERE status_out!=1 AND id_kamar='" + label7.Text + "' AND ((tgl_check_in<='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND tgl_check_out>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') OR (tgl_check_in<='" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' AND tgl_check_out >= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') OR ((tgl_check_in BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') AND (tgl_check_out BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')));";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                int jumlah_baris = 0;
                jumlah_baris = Convert.ToInt32(cmd.ExecuteScalar());
                if (jumlah_baris > 0)
                {
                    ada = true;
                }
                else
                {
                    ada = false;
                }

                if (ada == true)
                {
                    label11.ForeColor = Color.Red;
                    label11.Text = "*Maaf, rentang tanggal yang dimasukkan tidak tersedia kamar kosong!";
                    button1.Enabled = false;
                }
                else
                {
                    label11.ForeColor = Color.Green;
                    label11.Text = "*Tersedia kamar kosong untuk rentang tanggal yang dimasukkan!";
                    button1.Enabled = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (((dateTimePicker2.Value - dateTimePicker1.Value).TotalDays < 0) || ((dateTimePicker2.Value - DateTime.Now).TotalDays < 0))
            {
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }

            try
            {
                bool ada = false;
                string SQL = "SELECT COUNT(*) FROM reservasi WHERE status_out!=1 AND id_kamar='" + label7.Text + "' AND ((tgl_check_in<='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND tgl_check_out>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') OR (tgl_check_in<='" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' AND tgl_check_out >= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') OR ((tgl_check_in BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') AND (tgl_check_out BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "')));";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                int jumlah_baris = 0;
                jumlah_baris = Convert.ToInt32(cmd.ExecuteScalar());
                if (jumlah_baris > 0)
                {
                    ada = true;
                }
                else
                {
                    ada = false;
                }

                if (ada == true)
                {
                    label11.ForeColor = Color.Red;
                    label11.Text = "*Maaf, rentang tanggal yang dimasukkan tidak tersedia kamar kosong!";
                    button1.Enabled = false;
                }
                else
                {
                    label11.ForeColor = Color.Green;
                    label11.Text = "*Tersedia kamar kosong untuk rentang tanggal yang dimasukkan!";
                    button1.Enabled = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("konden ade kode ne brow..");
            try
            {
                string tgl_in, tgl_out;
                tgl_in = dateTimePicker1.Value.ToString("yyyy-M-d");
                tgl_out = dateTimePicker2.Value.ToString("yyyy-M-d");

                string SQL = "INSERT INTO reservasi (id_kamar, tgl_check_in, jam_check_in, tgl_check_out, jam_check_out, temp_bayar, status_out) VALUES ('" + label7.Text + "','" + tgl_in + "',null, '" + tgl_out + "',null, 0, -1);";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                conn.Close();
                MessageBox.Show("Booking Berhasil !", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.Rows.Clear();
                int mm = Convert.ToInt32(textBox2.Text);
                int yy = Convert.ToInt32(textBox1.Text);
                set_data(mm, yy);
                label9.Text = convert_string_bulan(mm) + " " + yy.ToString();
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

    
    }
}

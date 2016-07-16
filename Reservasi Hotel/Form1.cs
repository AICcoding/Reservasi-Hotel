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
    public partial class Form1 : Form
    {
        bool detik;
        public Form1()
        {
            InitializeComponent();
            cek_tgl_jam(DateTime.Now.ToString());
            label6.Text = "Home";
        }

        private void cek_tgl_jam(string date)
        {
            string[] tmp = date.Split(' ');
            string[] tmp_jam = tmp[1].Split(':');
            string am_pm = tmp[2];
            string hasil_tgl, hasil_jam1;

            hasil_jam1 = tmp_jam[0];

            hasil_tgl = tmp[0];

            label1.Text = hasil_tgl;
            if (Convert.ToInt32(tmp_jam[0]) < 10)
            {
                label2.Text = "0"+hasil_jam1;

            }
            else
            {
                label2.Text = hasil_jam1;
            }
            label4.Text = tmp_jam[1] + " " + am_pm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cari_no_kamar a = new cari_no_kamar();
            DialogResult dr = a.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            extra_bed a = new extra_bed();
            DialogResult dr = a.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            check_in a = new check_in();
            DialogResult dr = a.ShowDialog();
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            check_in a = new check_in();
            DialogResult dr = a.ShowDialog();
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void kamarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kamar a = new kamar();
            DialogResult dr = a.ShowDialog();
        }

        private void konsumsiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            konsumsi a = new konsumsi();
            DialogResult dr = a.ShowDialog();
        }

        private void extraBedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extra_bed a = new extra_bed();
            DialogResult dr = a.ShowDialog();
        }        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(detik==false)
            {
                label3.Visible = false;
                detik = true;
                cek_tgl_jam(DateTime.Now.ToString());
            }
            else
            {
                label3.Visible = true;
                detik = false;
            }
        }

        private void semuaTarifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tarif a = new tarif();
            DialogResult dr = a.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Point lokasi = new Point(panel2.Location.X, panel2.Location.Y + panel1.Height + panel2.Height-13);
            buat_navigasi_tree_view();
            string pilihan = treeView1.SelectedNode.Text;
            switch (pilihan)
            {
                case "Booking" :
                    booking bo = new booking();
                    bo.StartPosition = FormStartPosition.Manual;
                    bo.Location = lokasi;
                    bo.Width = panel2.Width;
                    DialogResult dbo = bo.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Check in":
                    check_in ci = new check_in();
                    ci.StartPosition = FormStartPosition.Manual;
                    ci.Location = lokasi;
                    DialogResult dci = ci.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Check out":
                    /*cari_no_kamar co = new cari_no_kamar();
                    DialogResult dco = co.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;*/
                    check_out co = new check_out();
                    co.StartPosition = FormStartPosition.Manual;
                    co.Location = lokasi;
                    DialogResult dco = co.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Extra bed":
                    extra_bed eb = new extra_bed();
                    eb.StartPosition = FormStartPosition.Manual;
                    eb.Location = lokasi;
                    DialogResult deb = eb.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Room":
                    kamar r = new kamar();
                    r.StartPosition = FormStartPosition.Manual;
                    r.Location = lokasi;
                    DialogResult dr = r.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Price":
                    tarif p = new tarif();
                    p.StartPosition = FormStartPosition.Manual;
                    p.Location = lokasi;
                    DialogResult dp = p.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
                case "Consumption":
                    konsumsi c = new konsumsi();
                    c.StartPosition = FormStartPosition.Manual;
                    c.Location = lokasi;
                    DialogResult dc = c.ShowDialog();
                    treeView1.SelectedNode = null;
                    label6.Text = "Home>";
                    break;
            }
        }
        
        private void buat_navigasi_tree_view()
        {
            string dir = treeView1.SelectedNode.FullPath.ToString();
            string[] tmp_dir = dir.Split('\\');
            string hasil = "Home>";
            for (int i = 0; i < tmp_dir.Length; i++)
            {
                if(i<tmp_dir.Length-1)
                {
                    hasil += tmp_dir[i] + ">";
                }
                else
                {
                    hasil += tmp_dir[i];

                }
            }
            label6.Text = hasil;    
        }
    
    }
}

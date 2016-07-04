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
    public partial class cetak_biru : Form
    {
        MySqlConnection conn = conectionservice.getconection();

        public cetak_biru()
        {
            InitializeComponent();  
        }
    }
}

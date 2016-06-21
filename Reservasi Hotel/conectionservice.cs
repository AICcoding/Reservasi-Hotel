using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservasi_Hotel
{
    class conectionservice
    {
        public static MySqlConnection getconection()
        {
            MySqlConnection conn = null;
            try
            {
                string connstr = "server=localhost; database=reservasi; uid=root; password=;";
                conn = new MySqlConnection(connstr);

            }
            catch (MySqlException sqlx)
            {
                throw new Exception(sqlx.Message.ToString());
            }
            return conn;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    internal class Connexio
    {

        public MySqlConnection Conn
        {
            get { return connecta(); }
        }

        private MySqlConnection conn = null;


        private MySqlConnection connecta()
        {
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    return conn;
                }
                else
                {
                    conn.Open();
                    return conn;
                }
            }
            //Port=3307;
            string conexion = "Server=localhost;Database=libreria;User ID=root;Password=123456;Pooling=false;";
            conn = new MySqlConnection(conexion);
            conn.Open();
            return conn;
        }

        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

}
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
        private static String s_database = "1libreria";
        private static String s_user = "1root";
        private static String s_password = "1123456";
        public void setDatabase(String s) {
            if (s != "") {
                s_database = s;
            }
        }
        public void setUser(String s)
        {
            if (s != "")
            {
                s_user = s;
            }
        }
        public void setPassword(String s)
        {
            if (s != "")
            {
                s_password = s;
            }
        }
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
            string conexion = "Server=localhost;Database="+ s_database  + ";User ID="+ s_user + ";Password="+ s_password + ";Pooling=false;";
            conn = new MySqlConnection(conexion);
            try {
                conn.Open();
            }
            catch (Exception ex) {
                return null;
            }
           
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
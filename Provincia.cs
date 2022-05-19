using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Provincia
    {
        public int ID { get; set; }
        public String nombre { get; set; }

        private List<Municipio> municipios;

        public Provincia(int id, String nombre) {
            this.ID = id;
            this.nombre = nombre;


            Connexio connexio = new Connexio();
            string query = "SELECT id,municipi from municipis WHERE id_pro = " + this.ID;
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            //rdr.Read();
            municipios = new List<Municipio>();

            while (rdr.Read())
            {
                Municipio m = new Municipio(rdr.GetInt32(0), rdr.GetString(1));
                municipios.Add(m);
            }


        }

        public override string ToString()
        {
            return "[" + ID + "] " + nombre;
        }

        public List<Municipio> getMunicipios() {
            return this.municipios;
        }

    }
}

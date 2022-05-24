using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Municipio
    {
        public int ID { get; set; }
        public String nombre { get; set; }

        public Municipio(int id, String nombre)
        {
            this.ID = id;
            this.nombre = nombre;
        }

        public override string ToString()
        {
            return "[" + ID + "] " + nombre;
        }

    }
}

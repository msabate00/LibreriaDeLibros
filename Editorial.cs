using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Editorial
    {

        public int id { set; get; }
        public string cif { set; get; }
        public string nombre { set; get; }
        public string telefono { set; get; }
        public string direccion { set; get; }
        public string email { set; get; }
        

        public Editorial(int id, string cif, string nombre, string telefono,  string direccion, string email)
        {
            this.id = id;
            this.cif = cif;
            this.nombre = nombre;
            this.telefono = telefono;
            this.direccion = direccion;
            this.email = email;
        }

    }
}

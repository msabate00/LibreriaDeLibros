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
        public int codi_postal { set; get; }
        public int codi_provincia { set; get; }
        public int codi_municipi { set; get; }
        public string email { set; get; }

        public Editorial()
        {

        }

        public Editorial(int id, string cif, string nombre, string telefono,  string direccion, int codi_postal, int codi_provincia, int codi_municipi, string email)
        {
            this.id = id;
            this.cif = cif;
            this.nombre = nombre;
            this.telefono = telefono;
            this.direccion = direccion;
            this.codi_postal = codi_postal;
            this.codi_provincia = codi_provincia;
            this.codi_municipi = codi_municipi;
            this.email = email;
        }

        public override string ToString()
        {
            return "[" + this.id.ToString() + "] " + this.nombre;
        }

    }
}

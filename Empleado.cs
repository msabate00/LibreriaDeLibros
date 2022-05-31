using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Empleado
    {
        public int id { set; get; }
        public string nom { set; get; }
        public string apellidos { set; get; }
        public String fecha { set; get; }
        public int telefono { set; get; }
        public string email { set; get; }
        public string direccion { set; get; }
        public int codi_municipi { set; get; }
        public int codi_provincia { set; get; }

        public Empleado(int id, string nom, string apellidos, String fecha, int telefono, string email, string direccion, int codi_municipi, int codi_provincia)
        {
            this.id = id;
            this.nom = nom;
            this.apellidos = apellidos;
            this.fecha = fecha;
            this.telefono = telefono;
            this.email = email;
            this.direccion = direccion;
            this.codi_municipi = codi_municipi;
            this.codi_provincia = codi_provincia;
        }
        public override string ToString()
        {
            return "[" + id + "] " + nom + " " + apellidos;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Cliente
    {
        public int id { set; get; }
        public string nif { set; get; }
        public string nom { set; get; }
        public string apellidos { set; get; }
        public DateTime fechaN { set; get; }
        public int telefono { set; get; }
        public string email { set; get; }
        public string direccion { set; get; }
        public int codi_poblacio { set; get; }
        public int codi_provincia { set; get; }

        public Cliente(int id, string nif, string nom, string apellidos, DateTime fechaN, int telefono, string email, string direccion, int codi_poblacio, int codi_provincia)
        {
            this.id = id;
            this.nif = nif;
            this.nom = nom;
            this.apellidos = apellidos;
            this.fechaN = fechaN.Date;
            this.telefono = telefono;
            this.email = email;
            this.direccion = direccion;
            this.codi_poblacio = codi_poblacio;
            this.codi_provincia = codi_provincia;
        }

    }
}
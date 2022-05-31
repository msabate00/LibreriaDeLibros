using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Factura
    {
        public Factura(int id, int cliente, int empleado, DateTime fecha, float preciofinal)
        {
            this.id = id;
            this.cliente = cliente;
            this.empleado = empleado;
            this.fecha = fecha;
            this.preciofinal = preciofinal;
        }

        public int id { set; get; }
        public int cliente { set; get; }
        public int empleado { set; get; }
        public DateTime fecha { set; get; }
        public float preciofinal { set; get; }
        public List<LiniaFactura> linias { set; get; }

        public override string ToString()
        {
            return "[" + id + "]  C: " + cliente +" -  E: "+ empleado;
        }

    }
}

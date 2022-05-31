using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class LinFacturas
    {
        public LinFacturas(int id_Factura, int id_Libro, int cantidad, float precioUnidad)
        {
            this.id_Factura = id_Factura;
            this.id_Libro = id_Libro;
            this.cantidad = cantidad;
            this.precioUnidad = precioUnidad;
            this.id = id_Factura + "_" + id_Libro;
        }

        public String id;
        public int id_Factura { set; get; }
        public int id_Libro { set; get; }
        public int cantidad { set; get; }
        public float precioUnidad { set; get; }

        public String GetId()
        {
            return this.id;
        }

        public void SetId(int id_Factura, int id_Libro)
        {
           this.id = id_Factura + "_" + id_Libro;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Pedido
    {
        public Pedido(int id, int editorialId, string? editorial, DateTime fechaPedido, DateTime fechaEntrega, int cantidad, double precioFinal)
        {
            Id = id;
            EditorialId = editorialId;
            EditorialNom = editorial;
            FechaPedido = fechaPedido;
            FechaEntrega = fechaEntrega;
            Cantidad = cantidad;
            PrecioFinal = precioFinal;
        }

        public Pedido()
        {

        }

        public int Id { get; set; }
        public int EditorialId { get; set; }
        public string? EditorialNom { get; set; }

        public DateTime FechaPedido { get; set; }

        public DateTime FechaEntrega { get; set; }

        public int Cantidad { get; set; }

        public double PrecioFinal { get; set; }


        public override string ToString()
        {
            return $"Pedido[id={Id}, editorialID={EditorialId}, fechaPedido={FechaPedido}, fechaEntrega={FechaEntrega}," +
                $"cantidad={Cantidad}, precioFinal={PrecioFinal}]";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class LineaPedido
    {
        public LineaPedido(int pedidoId, int libroId, String libroNom, int cantidad, double precio)
        {
            PedidoId = pedidoId;
            LibroId = libroId;
            LibroNom = libroNom;
            Cantidad = cantidad;
            Precio = precio;
        }

        public LineaPedido()
        {

        }

        public int Counter { get; set; }
        public int PedidoId { get; set; }
        public int LibroId { get; set; }
        public String? LibroNom { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }

        public override string ToString()
        {
            return $"LineaPedido[PedidoId={PedidoId}, LibroId={LibroId}, LibroNom={LibroNom}, cantidad={Cantidad}, Precio={Precio}]";
        }
    }


}

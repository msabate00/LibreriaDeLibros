using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LibreriaDeLibrosSL
{
    public class PedidoDao
    {
        Connexio connexio = new Connexio();

        public List<Pedido> getAllPedidos()
        {
            List<Pedido> pedidos = new List<Pedido>();
            string query = "select * from pedido_editorial;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Pedido pedido = new Pedido();
                pedido.Id = rdr["id"] != DBNull.Value ? Convert.ToInt32(rdr["id"]) : 0;
                pedido.EditorialId = rdr["editorial_id"] != DBNull.Value ? Convert.ToInt32(rdr["editorial_id"]) : 0;
                pedido.FechaPedido = rdr["fecha_pedido"] != DBNull.Value ? Convert.ToDateTime(rdr["fecha_pedido"]) : DateTime.Now;
                pedido.FechaEntrega = rdr["fecha_entrega"] != DBNull.Value ? Convert.ToDateTime(rdr["fecha_entrega"]) : DateTime.Now;
                pedido.Cantidad = rdr["cantidad"] != DBNull.Value ? Convert.ToInt32(rdr["cantidad"]) : 0;
                pedido.PrecioFinal = rdr["precio_final"] != DBNull.Value ? Convert.ToDouble(rdr["precio_final"]) : 0;

                pedidos.Add(pedido);
            }
            rdr.Close();
            connexio.Close();
            return pedidos;
        }

        public string getEditorialNameById(int id)
        {
            string name = "";
            string query = $"select nombre from editoriales where id = {id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                name = (string)result;
            }
            connexio.Close();
            return name;
        }

        public void eliminarPedidoById(int id)
        {
            string query = $"delete from pedido_editorial where id = {id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            cmd.ExecuteNonQuery();
            connexio.Close();
        }

        public void eliminarLineaPedidoByIdLibro(int id)
        {
            string query = $"delete from linea_pedido where libro_id = {id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            cmd.ExecuteNonQuery();
            connexio.Close();
        }

        public void eliminarLineaPedidoByIdPedido(int id)
        {
            string query = $"delete from linea_pedido where pedido_id = {id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            cmd.ExecuteNonQuery();
            connexio.Close();
        }

        public void eliminarLineaPedidoByIdPedidoAndIdLibro(int idPedido, int idLibro)
        {
            string query = $"delete from linea_pedido where libro_id = {idLibro} and pedido_id = {idPedido};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            cmd.ExecuteNonQuery();
            connexio.Close();
        }

        public void insertarPedido(Pedido p)
        {
            if (p != null)
            {
                string query = "INSERT INTO pedido_editorial (editorial_id, fecha_pedido, fecha_entrega, cantidad" +
                ", precio_final) VALUES(@editorial_id, @fecha_pedido, @fecha_entrega, @cantidad, @precio_final)";
                MySqlCommand command = new MySqlCommand(query, connexio.Conn);

                command.Parameters.AddWithValue("@editorial_id", p.EditorialId);
                command.Parameters.AddWithValue("@fecha_pedido", p.FechaPedido);
                command.Parameters.AddWithValue("@fecha_entrega", p.FechaEntrega);
                command.Parameters.AddWithValue("@cantidad", p.Cantidad);
                command.Parameters.AddWithValue("@precio_final", p.PrecioFinal);

                command.ExecuteNonQuery();
                connexio.Close();
            }
        }

        public void updatePedido(Pedido p)
        {
            if (p != null)
            {
                string query = "UPDATE pedido_editorial SET editorial_id = @editorial_id, fecha_pedido = @fecha_pedido" +
                    ", fecha_entrega = @fecha_entrega, cantidad = @cantidad, precio_final = @precio_final WHERE id = @id ";
                MySqlCommand command = new MySqlCommand(query, connexio.Conn);

                command.Parameters.AddWithValue("@id", p.Id);
                command.Parameters.AddWithValue("@editorial_id", p.EditorialId);
                command.Parameters.AddWithValue("@fecha_pedido", p.FechaPedido);
                command.Parameters.AddWithValue("@fecha_entrega", p.FechaEntrega);
                command.Parameters.AddWithValue("@cantidad", p.Cantidad);
                command.Parameters.AddWithValue("@precio_final", p.PrecioFinal);

                command.ExecuteNonQuery();
                connexio.Close();
            }
        }

        public void insertarLineaPedido(int pedidoId, LineaPedido lineaP)
        {
            if (lineaP != null)
            {
                string query = "INSERT INTO linea_pedido (pedido_id, libro_id, cantidad, precio_proveedor_unit) " +
                    "VALUES(@pedido_id, @libro_id, @cantidad, @precio_proveedor_unit)";
                MySqlCommand command = new MySqlCommand(query, connexio.Conn);

                command.Parameters.AddWithValue("@pedido_id", pedidoId);
                command.Parameters.AddWithValue("@libro_id", lineaP.LibroId);
                command.Parameters.AddWithValue("@cantidad", lineaP.Cantidad);
                command.Parameters.AddWithValue("@precio_proveedor_unit", lineaP.Precio);

                command.ExecuteNonQuery();
                connexio.Close();
            }
        }

        public void insertarLineaPedidoWithPedidoId(int pedidoId, LineaPedido lineaP)
        {
            if (lineaP != null)
            {
                string query = "INSERT INTO linea_pedido (pedido_id, libro_id, cantidad, precio_proveedor_unit) " +
                    "VALUES(@pedido_id, @libro_id, @cantidad, @precio_proveedor_unit)";
                MySqlCommand command = new MySqlCommand(query, connexio.Conn);

                command.Parameters.AddWithValue("@pedido_id", pedidoId);
                command.Parameters.AddWithValue("@libro_id", lineaP.LibroId);
                command.Parameters.AddWithValue("@cantidad", lineaP.Cantidad);
                command.Parameters.AddWithValue("@precio_proveedor_unit", lineaP.Precio);

                command.ExecuteNonQuery();
                connexio.Close();
            }
        }

        public void updateLineaPedido(int pedidoId, LineaPedido lineaP)
        {
            if (lineaP != null)
            {
                string query = "UPDATE linea_pedido SET cantidad = @cantidad, precio_proveedor_unit = @precio where pedido_id = @pedidoId and libro_id = @libroId";
                MySqlCommand command = new MySqlCommand(query, connexio.Conn);

                command.Parameters.AddWithValue("@cantidad", lineaP.Cantidad);
                command.Parameters.AddWithValue("@precio", lineaP.Precio);
                command.Parameters.AddWithValue("@pedidoId", pedidoId);
                command.Parameters.AddWithValue("@libroId", lineaP.LibroId);
                command.ExecuteNonQuery();
                connexio.Close();
            }
        }

        public ObservableCollection<LineaPedido> GetLineasByPedidoId(Pedido p)
        {
            int counter = 1;
            ObservableCollection<LineaPedido> lineas = new ObservableCollection<LineaPedido>();
            string query = $"select libro_id, cantidad, precio_proveedor_unit from linea_pedido where pedido_id = {p.Id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                LineaPedido lpedido = new LineaPedido();
                lpedido.Counter = counter;
                lpedido.PedidoId = p.Id;
                lpedido.LibroId = rdr["libro_id"] != DBNull.Value ? Convert.ToInt32(rdr["libro_id"]) : 0;
                lpedido.Cantidad = rdr["cantidad"] != DBNull.Value ? Convert.ToInt32(rdr["cantidad"]) : 0;
                lpedido.Precio = rdr["precio_proveedor_unit"] != DBNull.Value ? Convert.ToDouble(rdr["precio_proveedor_unit"]) : 0;

                lineas.Add(lpedido);
                counter++;
            }
            rdr.Close();
            connexio.Close();
            return lineas;
        }

        public List<Editorial> GetEditoriales()
        {
            List<Editorial> editoriales = new List<Editorial>();
            string query = "select id, nombre from editoriales;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Editorial editorial = new Editorial();
                editorial.id = rdr["id"] != DBNull.Value ? Convert.ToInt32(rdr["id"]) : 0;
                editorial.nombre = rdr["nombre"] != DBNull.Value ? Convert.ToString(rdr["nombre"]) : "";

                editoriales.Add(editorial);
            }
            rdr.Close();
            connexio.Close();
            return editoriales;
        }

        public string GetLibroNameById(int id)
        {
            string name = "";
            string query = $"select titulo from libros where id = {id};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                name = (string)result;
            }
            connexio.Close();
            return name;
        }

        public List<Libro> GetLibros()
        {
            List<Libro> libros = new List<Libro>();
            string query = "select id, titulo from libros;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Libro libro = new Libro();
                libro.ID = rdr["id"] != DBNull.Value ? Convert.ToInt32(rdr["id"]) : 0;
                libro.Titulo = rdr["titulo"] != DBNull.Value ? Convert.ToString(rdr["titulo"]) : "";
                //libro.Precio = rdr["precio"] != DBNull.Value ? Convert.ToDouble(rdr["precio"]) : 0;

                libros.Add(libro);
            }
            rdr.Close();
            connexio.Close();
            return libros;
        }

        public int GetMaxIdPedido()
        {
            int id = 0;
            string query = $"SELECT MAX(id) FROM pedido_editorial";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                id = (int)result;
            }
            connexio.Close();
            return id;
        }
    }
}

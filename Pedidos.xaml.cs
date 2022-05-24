using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Lógica de interacción para Pedidos.xaml
    /// </summary>
    public partial class Pedidos : Page
    {
        Connexio connexio = new Connexio();
        List<Pedido> lista_pedidos = new List<Pedido>();
        public Pedidos()
        {
            InitializeComponent();
            loadData();
        }


        public void loadData()
        {
            string query = "select * from pedidos;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Pedido pedido = new Pedido();
                pedido.Id = rdr["id"] != DBNull.Value ? Convert.ToInt32(rdr["id"]) : 0;
                pedido.EditorialId = rdr["editorial_id"] != DBNull.Value ? Convert.ToInt32(rdr["editorial_id"]) : 0;
                pedido.EditorialNom = $"{pedido.EditorialId} " + getEditorialNameById(pedido.EditorialId);
                pedido.FechaPedido = rdr["fecha_pedido"] != DBNull.Value ? Convert.ToDateTime(rdr["fecha_pedido"]) : DateTime.Now;
                pedido.FechaEntrega = rdr["fecha_entrega"] != DBNull.Value ? Convert.ToDateTime(rdr["fecha_entrega"]) : DateTime.Now;
                pedido.Cantidad = rdr["cantidad"] != DBNull.Value ? Convert.ToInt32(rdr["cantidad"]) : 0;
                pedido.PrecioFinal = rdr["precio_final"] != DBNull.Value ? Convert.ToDouble(rdr["precio_final"]) : 0;

                lista_pedidos.Add(pedido);
            }
            rdr.Close();
            connexio.Close();
            lvpedidos.ItemsSource = lista_pedidos;
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

        private void onNuevo(object sender, RoutedEventArgs e)
        {
            EditorialesPopup window = new EditorialesPopup(EditorialesPopup.forma.insertar);
            window.ShowDialog();
            loadData();
        }
        private void onEditar(object sender, RoutedEventArgs e)
        {
            EditorialesPopup window = new EditorialesPopup(EditorialesPopup.forma.modificar);
            window.ShowDialog();
            loadData();
        }

        private void onEliminar(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Seguro que quieres eliminar el pedido?",
          "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Close the window  
            }
        }

        private void onActualizar(object sender, RoutedEventArgs e)
        {
            loadData();
        }
    }
}

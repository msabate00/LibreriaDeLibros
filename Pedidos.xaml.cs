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
        PedidoDao pedidosDao = new PedidoDao();
        List<Pedido> lista_pedidos = new List<Pedido>();
        public Pedidos()
        {
            InitializeComponent();
            loadData();
        }


        public void loadData()
        {
            lista_pedidos.Clear();
            try
            {
                lista_pedidos = pedidosDao.getAllPedidos();
                setEditorialName();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        public void setEditorialName()
        {
            foreach (var pedido in lista_pedidos)
            {
                pedido.EditorialNom = pedidosDao.getEditorialNameById(pedido.EditorialId);
            }
            lvpedidos.ItemsSource = lista_pedidos;
        }

        private void onNuevo(object sender, RoutedEventArgs e)
        {
            openEditWindow(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onEditar(object sender, RoutedEventArgs e)
        {
            if (lvpedidos.SelectedItem != null)
            {
                Pedido select = lvpedidos.SelectedItem as Pedido;
                openEditWindow(select);
            }
            else
            {
                MessageBox.Show("Selecciona un pedido!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// Eliminar un pedido por id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onEliminar(object sender, RoutedEventArgs e)
        {
            if (lvpedidos.SelectedItem != null)
            {
                Pedido select = lvpedidos.SelectedItem as Pedido;
                MessageBoxResult result = MessageBox.Show($"Seguro que quieres eliminar el pedido {select.Id}?",
                  "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        pedidosDao.eliminarLineaPedidoByIdPedido(select.Id);
                        pedidosDao.eliminarPedidoById(select.Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un pedido!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// Abrir ventana Linea Pedido
        /// </summary>
        public void openEditWindow(Pedido p)
        {
            LineaPedidoView lp = new LineaPedidoView();
            if (p != null)
            {
                lp = new LineaPedidoView(p);
                lp.EditTitol.Content = "Editar";
            }
            lp.ShowDialog();

            /*
            MainWindow window = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (window != null)
                window.rightCol.Source = new Uri("LineaPedidoView.xaml", UriKind.RelativeOrAbsolute);
            */
        }

        private void onActualizar(object sender, RoutedEventArgs e)
        {
            loadData();
        }
    }
}

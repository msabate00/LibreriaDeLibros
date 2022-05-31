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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Frame middleFrame = rightCol;
            WelcomePage window = new WelcomePage();
            middleFrame.Source = new Uri("WelcomePage.xaml", UriKind.RelativeOrAbsolute);

        }

        private void onEditorialClick(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Editoriales window = new Editoriales();
            middleFrame.Source = new Uri("Editoriales.xaml", UriKind.RelativeOrAbsolute);
            

        }

        private void onPedidosClick(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Pedidos window = new Pedidos();
            middleFrame.Source = new Uri("Pedidos.xaml", UriKind.RelativeOrAbsolute);
        }

        private void onClientesClick(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Clientes window = new Clientes();
            middleFrame.Source = new Uri("Clientes.xaml", UriKind.RelativeOrAbsolute);
        }

        private void onFacturasClick(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Facturas window = new Facturas();
            middleFrame.Source = new Uri("Facturas.xaml", UriKind.RelativeOrAbsolute);
        }
        private void onLinFacturasClick(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            LiniaFactura window = new LiniaFactura();
            middleFrame.Source = new Uri("LiniaFactura.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Empleados window = new Empleados();
            window.Show();

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Libros window = new Libros();
            middleFrame.Source = new Uri("Libros.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}

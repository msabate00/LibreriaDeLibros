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
using System.Windows.Shapes;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Lógica de interacción para Factura.xaml
    /// </summary>
    public partial class Facturas : Window
    {
        Connexio connexio = new Connexio();
        List<Factura> lista_facturas = new List<Factura>();
        public Facturas()
        {
            InitializeComponent();
            cargar();
        }

        private void onInsertar(object sender, RoutedEventArgs e)
        {
          //  EditorialesPopup window = new EditorialesPopup();
          //  window.ShowDialog();
        }

        public void cargar()
        {
            string query = "select* from facturas;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            lista_facturas = new List<Factura>();
            while (rdr.Read())
            {
                Factura prov = new Factura(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetFloat(4));
                lista_facturas.Add(prov);
            }
            rdr.Close();
            lvfacturas.ItemsSource = lista_facturas;
        }

    }
}

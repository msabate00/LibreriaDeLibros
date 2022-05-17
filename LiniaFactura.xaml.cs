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
    /// Lógica de interacción para LiniaFactura.xaml
    /// </summary>
    public partial class LiniaFactura : Window
    {
        Connexio connexio = new Connexio();
        List<LinFacturas> lista_LinFacturas = new List<LinFacturas>();
        public LiniaFactura()
        {
            InitializeComponent();
            cargar();
        }

        private void onInsertar(object sender, RoutedEventArgs e)
        {
            EditorialesPopup window = new EditorialesPopup();
            window.ShowDialog();
        }

        public void cargar()
        {
            string query = "select* from facturas;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            lista_LinFacturas = new List<LinFacturas>();
            while (rdr.Read())
            {
                LinFacturas prov = new LinFacturas(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetFloat(3));
                lista_LinFacturas.Add(prov);
            }
            rdr.Close();
            lvLiniaFacturas.ItemsSource = lista_LinFacturas;
        }
    }
}

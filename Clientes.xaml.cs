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
using MySql.Data.MySqlClient;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        Connexio connexio = new Connexio();
        List<Cliente> lista_clientes = new List<Cliente>();

        public Clientes()
        {
            InitializeComponent();
            cargarClientes();
        }

        private void cargarClientes()
        {
            string query = "select * from clientes;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            lista_clientes = new List<Cliente>();
            while (rdr.Read())
            {
                DateTime d = new DateTime(rdr.GetDateTime(4).Date.Year, rdr.GetDateTime(4).Date.Month, rdr.GetDateTime(4).Date.Day);
                Cliente cli = new Cliente(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), d.Date, rdr.GetInt32(5), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetInt32(9));
                lista_clientes.Add(cli);
            }
            rdr.Close();
            lvClientes.ItemsSource = lista_clientes;
        }

        private void btnAddCli_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AddCliente v = new AddCliente();
            v.ShowDialog();
            Show();
            lvClientes.ItemsSource = null;
            cargarClientes();
        }

        private void btnModCli_Click(object sender, RoutedEventArgs e)
        {
            if(lvClientes.SelectedItem != null)
            {                
                Cliente c = lvClientes.SelectedItem as Cliente;
                Hide();
                ModCliente v = new ModCliente(c);
                v.ShowDialog();
                Show();
                lvClientes.ItemsSource = null;
                cargarClientes();
            }
            else
            {
                MessageBox.Show("No hay ningún cliente seleccionado");
            }
        }

        private void btnDelCli_Click(object sender, RoutedEventArgs e)
        {
            if (lvClientes.SelectedItem != null)
            {
                Cliente c = lvClientes.SelectedItem as Cliente;

                MySqlConnection conn = connexio.Conn;

                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "DELETE FROM clientes WHERE id=?id";
                comm.Parameters.Add("?id", MySqlDbType.Int32).Value = c.id;
                comm.ExecuteNonQuery();

                lvClientes.ItemsSource = null;
                cargarClientes();
            }
            else
            {
                MessageBox.Show("No hay ningún cliente seleccionado");
            }
        }
    }
}

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
using MySql.Data.MySqlClient;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Lógica de interacción para AddCliente.xaml
    /// </summary>
    public partial class AddCliente : Window
    {
        Connexio connexio = new Connexio();

        public AddCliente()
        {
            InitializeComponent();
            comboProv();
        }

        private void comboProv()
        {
            string query = "select * from provincies;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                String prov = rdr.GetString(2);

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Name = "_" + id;
                cbi.Content = prov;
                
                provCli.Items.Add(cbi);
            }
            rdr.Close();
        }

        private void provChange(object sender, RoutedEventArgs e)
        {
            String idM = "";

            idM = ((sender as ComboBox).SelectedItem as ComboBoxItem).Name as string;
            idM = idM.Substring(1, idM.Length - 1);

            pobCli.IsEnabled = true;
            pobCli.Items.Clear();

            string query = "select * from municipis where id_pro = " + idM + ";";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                String prov = rdr.GetString(2);

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Name = "_" + id;
                cbi.Content = prov;

                pobCli.Items.Add(cbi);
            }
            rdr.Close();
        }

        private void btnAddCli_Click(object sender, RoutedEventArgs e)
        {
            if (nifCli.Text != "" && nomCli.Text != "" && apelCli.Text != "" && fechaNCli.Text != ""
                && tlfCli.Text != "" && mailCli.Text != "" && dirCli.Text != "" && provCli.SelectedItem != null
                && pobCli.SelectedItem != null)
            {
                String nif = nifCli.Text;
                String nom = nomCli.Text;
                String apel = apelCli.Text;
                DateTime fechaN = Convert.ToDateTime(fechaNCli.Text);
                String tlf = tlfCli.Text;
                String email = mailCli.Text;
                String dir = dirCli.Text;
                String idProv = (provCli.SelectedItem as ComboBoxItem).Name;
                String idPob = (pobCli.SelectedItem as ComboBoxItem).Name;

                idProv = idProv.Substring(1, idProv.Length - 1);
                idPob = idPob.Substring(1, idPob.Length - 1);

                MySqlConnection conn = connexio.Conn;

                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO clientes(nif,nombre,apellidos,fecha_nacimiento,telefono,email,direccion,codi_municipi,codi_provincia) VALUES(?nif,?nombre,?apellidos,?fecha_nacimiento,?telefono,?email,?direccion,?codi_municipi,?codi_provincia)";
                comm.Parameters.Add("?nif", MySqlDbType.VarChar).Value = nif;
                comm.Parameters.Add("?nombre", MySqlDbType.VarChar).Value = nom;
                comm.Parameters.Add("?apellidos", MySqlDbType.VarChar).Value = apel;
                comm.Parameters.Add("?fecha_nacimiento", MySqlDbType.DateTime).Value = fechaN;
                comm.Parameters.Add("?telefono", MySqlDbType.Int32).Value = Int32.Parse(tlf);
                comm.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
                comm.Parameters.Add("?direccion", MySqlDbType.VarChar).Value = dir;
                comm.Parameters.Add("?codi_municipi", MySqlDbType.Int32).Value = Int32.Parse(idPob);
                comm.Parameters.Add("?codi_provincia", MySqlDbType.Int32).Value = Int32.Parse(idProv);
                comm.ExecuteNonQuery();

                Close();
            }
            else
            {
                MessageBox.Show("Los datos de inserción son incorrectos, por favor revisalos.");
            }
        }
    }
}

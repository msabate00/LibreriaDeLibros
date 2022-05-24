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
    /// Lógica de interacción para ModCliente.xaml
    /// </summary>
    public partial class ModCliente : Window
    {
        private Cliente modcli;
        Connexio connexio = new Connexio();
        bool carga = true;

        public ModCliente(Cliente c)
        {
            InitializeComponent();
            modcli = c;
            rellenarCampos();
        }

        private void rellenarCampos()
        {
            nifCli.Text = modcli.nif;
            nomCli.Text = modcli.nom;
            apelCli.Text = modcli.apellidos;
            fechaNCli.SelectedDate = modcli.fechaN;
            tlfCli.Text = modcli.telefono.ToString();
            mailCli.Text = modcli.email;
            dirCli.Text = modcli.direccion;
            idCli.Content = modcli.id;

            int idProv = modcli.codi_provincia;
            int idPob = modcli.codi_poblacio;

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

                if (id == idProv)
                {
                    provCli.SelectedItem = cbi;
                }
            }
            rdr.Close();

            pobCli.IsEnabled = true;
            pobCli.Items.Clear();

            string query2 = "select * from municipis where id_pro = " + idProv + ";";
            MySqlCommand cmd2 = new MySqlCommand(query2, connexio.Conn);
            MySqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                int id = rdr2.GetInt32(0);
                String prov = rdr2.GetString(2);

                ComboBoxItem cbi2 = new ComboBoxItem();
                cbi2.Name = "_" + id;
                cbi2.Content = prov;

                pobCli.Items.Add(cbi2);

                if (id == idPob)
                {
                    pobCli.SelectedItem = cbi2;
                }
            }
            rdr2.Close();

            carga = false;
        }

        private void provChange(object sender, RoutedEventArgs e)
        {
            if (!carga)
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
        }

        private void btnModCli_Click(object sender, RoutedEventArgs e)
        {
            if (nifCli.Text != "" && nomCli.Text != "" && apelCli.Text != "" && fechaNCli.Text != ""
                && tlfCli.Text != "" && mailCli.Text != "" && dirCli.Text != "" && provCli.SelectedItem != null
                && pobCli.SelectedItem != null && idCli.Content != null)
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
                int id = (int)idCli.Content;

                idProv = idProv.Substring(1, idProv.Length - 1);
                idPob = idPob.Substring(1, idPob.Length - 1);

                MySqlConnection conn = connexio.Conn;

                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "UPDATE clientes SET nif=?nif,nombre=?nombre,apellidos=?apellidos,fecha_nacimiento=?fecha_nacimiento,telefono=?telefono,email=?email,direccion=?direccion,codi_municipi=?codi_municipi,codi_provincia=?codi_provincia WHERE id=?id;";
                comm.Parameters.Add("?nif", MySqlDbType.VarChar).Value = nif;
                comm.Parameters.Add("?nombre", MySqlDbType.VarChar).Value = nom;
                comm.Parameters.Add("?apellidos", MySqlDbType.VarChar).Value = apel;
                comm.Parameters.Add("?fecha_nacimiento", MySqlDbType.DateTime).Value = fechaN;
                comm.Parameters.Add("?telefono", MySqlDbType.Int32).Value = Int32.Parse(tlf);
                comm.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
                comm.Parameters.Add("?direccion", MySqlDbType.VarChar).Value = dir;
                comm.Parameters.Add("?codi_municipi", MySqlDbType.Int32).Value = Int32.Parse(idPob);
                comm.Parameters.Add("?codi_provincia", MySqlDbType.Int32).Value = Int32.Parse(idProv);
                comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                comm.ExecuteNonQuery();

                Close();
            }
            else
            {
                MessageBox.Show("Los datos de modificación son incorrectos, por favor revisalos.");
            }
        }
    }
}

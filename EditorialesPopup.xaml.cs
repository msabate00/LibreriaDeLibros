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
    /// Lógica de interacción para EditorialesPopup.xaml
    /// </summary>
    public partial class EditorialesPopup : Window
    {
        private forma _forma;
        private int id_tratar = -1;
        private List<Provincia> provincias;
        public enum forma { 
            insertar = 0, modificar = 1, eliminar = 2
        }
        public EditorialesPopup(forma f)
        {
            _forma = f;
            
            InitializeComponent();
            if (_forma == forma.insertar)
            {
                tb_editorial_popup_id.IsEnabled = false;
            }
            cargar();
        }
        public EditorialesPopup(forma f, int id)
        {
            _forma = f;
            id_tratar = id;
            InitializeComponent();
            cargar();

            Connexio connexio = new Connexio();
            if (f == forma.modificar)
            {
                tb_editoriales_popup_titulo.Content = "Modificar Editorial";
            }
            else {
                tb_editoriales_popup_titulo.Content = "Eliminar Editorial \n" +
                    "Estas seguro que quieres eliminarla?";
            }
            
            string query = "SELECT * from editoriales WHERE id = " + id;
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();

           

            Editorial editorial = new Editorial(
                rdr.GetInt32(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetInt32(5),
                rdr.GetInt32(6),
                rdr.GetInt32(7),
                rdr.GetString(8));

            tb_editorial_popup_id.Text = editorial.id.ToString();
            tb_editorial_popup_id.IsReadOnly = true;
            tb_editorial_popup_cif.Text = editorial.cif;
            tb_editorial_popup_cp.Text = editorial.codi_postal.ToString();
            tb_editorial_popup_direccion.Text = editorial.direccion;
            tb_editorial_popup_email.Text = editorial.email;
            tb_editorial_popup_nombre.Text = editorial.nombre;
            tb_editorial_popup_telefono.Text = editorial.telefono;

            ItemCollection icP = cb_editorial_popup_provincia.Items;
            for (int i = 0; i < icP.Count; i++) {
                if ((icP[i] as Provincia).ID == editorial.codi_provincia) {
                    cb_editorial_popup_provincia.SelectedItem = icP[i];
                    break;
                }
            }

            ItemCollection icM = cb_editorial_popup_municipio.Items;
            for (int i = 0; i < icM.Count; i++)
            {
                if ((icM[i] as Municipio).ID == editorial.codi_municipi)
                {
                    cb_editorial_popup_municipio.SelectedItem = icM[i];
                    break;
                }
            }

           



            



            connexio.Close();

           
        }

        private void cargar() {
            Connexio connexio = new Connexio();
            string query = "SELECT id,provincia from provincies";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            provincias = new List<Provincia>();

            while (rdr.Read())
            {
                Provincia p = new Provincia(rdr.GetInt32(0), rdr.GetString(1));
                provincias.Add(p);
            }
            cb_editorial_popup_provincia.ItemsSource = provincias;
            
            connexio.Close();
        }

        private void onAceptar(object sender, RoutedEventArgs e)
        {
            Connexio connexio = new Connexio();
            try
            {

                switch (_forma)
                {
                    case forma.insertar:
                        //INSERT INTO editoriales (cif, nombre, telefono, direccion, codi_postal, codi_provincia , codi_municipi, email) 
                        //VALUES('U23747413', 'Áglaya', '968320680', 'C/ Real, 16 - Bajo', 30201, 44, 244127, 'info@editorialaglaya.com');
                        //'U23747413', 'Áglaya', '968320680', 'C/ Real, 16 - Bajo', 30201, 44, 244127, 'info@editorialaglaya.com'
                        string query = "INSERT INTO editoriales (cif, nombre, telefono, direccion, codi_postal, codi_provincia , codi_municipi, email) " +
                            "VALUES('" + tb_editorial_popup_cif.Text + "','" + tb_editorial_popup_nombre.Text + "'," +
                            "'" + tb_editorial_popup_telefono.Text + "','" + tb_editorial_popup_direccion.Text + "'," +
                            "" + tb_editorial_popup_cp.Text + "," +
                            (cb_editorial_popup_provincia.SelectedItem as Provincia).ID + "," +
                            "" + (cb_editorial_popup_municipio.SelectedItem as Municipio).ID + ",'" + tb_editorial_popup_email.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
                        cmd.ExecuteNonQuery();
                        



                        break;
                    case forma.modificar:

                        String query2 = "UPDATE editoriales SET cif = '" + tb_editorial_popup_cif.Text +
                            "', nombre = '" + tb_editorial_popup_nombre.Text + 
                            "', telefono = '"+ tb_editorial_popup_telefono.Text + 
                            "', direccion = '"+ tb_editorial_popup_direccion.Text + 
                            "', codi_postal = '"+ tb_editorial_popup_cp.Text + 
                            "', codi_provincia = '"+ (cb_editorial_popup_provincia.SelectedItem as Provincia).ID + 
                            "', codi_municipi = '"+ (cb_editorial_popup_municipio.SelectedItem as Municipio).ID + 
                            "', email = '"+ tb_editorial_popup_email.Text + 
                            "' WHERE id = " + id_tratar;

                       MySqlCommand cmd2 = new MySqlCommand(query2, connexio.Conn);
                        cmd2.ExecuteNonQuery();



                        break;
                    case forma.eliminar:

                        String query3 = "DELETE FROM editoriales WHERE id = " + id_tratar;
                        MySqlCommand cmd3 = new MySqlCommand(query3, connexio.Conn);
                        cmd3.ExecuteNonQuery();

                        break;

                }

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
            
            connexio.Close();
            this.Close();
        }

        private void onSelectProvincia(object sender, SelectionChangedEventArgs e)
        {
            cb_editorial_popup_municipio.IsEnabled = true;
            cb_editorial_popup_municipio.ItemsSource = 
            ((sender as ComboBox).SelectedItem as Provincia).getMunicipios();
        }
    }
}

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
    /// Lógica de interacción para Editoriales.xaml
    /// </summary>
   
    public partial class Editoriales : Page
    {
        Connexio connexio = new Connexio();
        List<Editorial> lista_editoriales = new List<Editorial>();
        public Editoriales()
        {
            InitializeComponent();
            cargar();
        }


        public void cargar()
        {
            string query = "select * from editoriales;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            lista_editoriales = new List<Editorial>();
            while (rdr.Read())
            {
               Editorial prov = new Editorial(
                   rdr.GetInt32(0),
                   rdr.GetString(1),
                   rdr.GetString(2),
                   rdr.GetString(3), 
                   rdr.GetString(4),
                   rdr.GetInt32(5),
                   rdr.GetInt32(6),
                   rdr.GetInt32(7),
                   rdr.GetString(8));
                lista_editoriales.Add(prov);
            }
            rdr.Close();
            lveditoriales.ItemsSource = lista_editoriales;
        }



        private void onInsertar(object sender, RoutedEventArgs e)
        {
            EditorialesPopup window = new EditorialesPopup(EditorialesPopup.forma.insertar);
            window.ShowDialog();
            cargar();
        }
        private void onModificar(object sender, RoutedEventArgs e)
        {
            
            EditorialesPopup window = new EditorialesPopup(EditorialesPopup.forma.modificar, (lveditoriales.SelectedItem as Editorial).id);
            window.ShowDialog();
            cargar();
        }

        private void onEliminar(object sender, RoutedEventArgs e)
        {
            if (lveditoriales.SelectedItem != null)
            {
                EditorialesPopup window = new EditorialesPopup(EditorialesPopup.forma.eliminar, (lveditoriales.SelectedItem as Editorial).id);
                window.ShowDialog();
                cargar();
            }
            else {
                MessageBox.Show("Debes de seleccionar una editorial para eliminarla");
            }
            
        }
    }
}

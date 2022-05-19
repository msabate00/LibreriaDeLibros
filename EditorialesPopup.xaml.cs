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
        private int id_tratar;
        public enum forma { 
            insertar = 0, modificar = 1, eliminar = 2
        }
        public EditorialesPopup(forma f)
        {
            _forma = f;
            if (_forma == forma.insertar) {
                tb_editorial_popup_id.IsEnabled = false;
            }
            InitializeComponent();
        }
        public EditorialesPopup(forma f, int id)
        {
            _forma = f;
            id_tratar = id;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Connexio connexio = new Connexio();
            switch (_forma) {
                case forma.insertar:
                    //INSERT INTO editoriales (cif, nombre, telefono, direccion, codi_postal, codi_provincia , codi_municipi, email) 
                    //VALUES('U23747413', 'Áglaya', '968320680', 'C/ Real, 16 - Bajo', 30201, 44, 244127, 'info@editorialaglaya.com');

                    string query = "INSERT INTO editoriales (cif, nombre, telefono, direccion, codi_postal, codi_provincia , codi_municipi, email) " +
                        "VALUES()";
                    MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
                    cmd.ExecuteNonQuery();
                    this.Close();


                    break;
                case forma.modificar:
                    break;
                case forma.eliminar:
                    break;

            }
        }
    }
}

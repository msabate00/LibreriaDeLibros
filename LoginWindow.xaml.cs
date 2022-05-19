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
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void onAceptarLogin(object sender, RoutedEventArgs e)
        {
            Connexio connexio = new Connexio();
            connexio.setDatabase(tb_login_db.Text);
            connexio.setUser(tb_login_user.Text);
            connexio.setPassword(tb_login_password.Text);

            if (connexio.Conn == null)
            {
                MessageBox.Show("No se ha podido conectar con la base de datos");
            }
            else {
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }

            
        }
    }
}

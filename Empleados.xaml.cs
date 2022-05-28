
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
    /// Lógica de interacción para Empleados.xaml
    /// </summary>
    public partial class Empleados : Window
{
    Connexio connexio = new Connexio();
    List<Empleado> lista_empleados = new List<Empleado>();
    public Empleados()
    {
        InitializeComponent();
        cargar();
    }

    private void btnAddCli_Click(object sender, RoutedEventArgs e)
    {
        Hide();
        Agregar_empleat v = new Agregar_empleat();
        v.ShowDialog();
        Show();
    }

    private void btnModCli_Click(object sender, RoutedEventArgs e)
    {
        Hide();
        Agregar_empleat v = new Agregar_empleat();
        v.ShowDialog();
        Show();
    }
    public void cargar()
    {
        string query = "select * from empleados;";
        MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        lista_empleados = new List<Empleado>();
        while (rdr.Read())
        {
            DateTime d = new DateTime(rdr.GetDateTime(3).Date.Year, rdr.GetDateTime(3).Date.Month, rdr.GetDateTime(3).Date.Day);
            Empleado prov = new Empleado(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), d, rdr.GetInt32(4), rdr.GetString(5), rdr.GetString(6), rdr.GetInt32(7), rdr.GetInt32(8));
            lista_empleados.Add(prov);
        }
        rdr.Close();
        lvEmpleados.ItemsSource = lista_empleados;
    }

}

}
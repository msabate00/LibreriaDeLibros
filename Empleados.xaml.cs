
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
    public partial class Empleados : Page
{
    Connexio connexio = new Connexio();
    List<Empleado> lista_empleados = new List<Empleado>();
        private List<Provincia> provincias;
        public Empleados()
    {
        InitializeComponent();
        cargar_empleados();
        cargar_provincia();

            tb_apellido.Text = "dqaf";
            tb_direccion.Text = "dqwd";
            tb_email.Text = "qawd@asd";
            tb_nacimiento.Text = "2018/11/10";
            tb_telefono.Text = "123456789";
            tb_nom.Text = "qwdw";

        }

 

   
    public void cargar_empleados()
    {
        string query = "select * from empleados;";
        MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
        MySqlDataReader rdr = cmd.ExecuteReader();
        lista_empleados = new List<Empleado>();
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;
            while (rdr.Read())
        {

                DateTime d = new DateTime(rdr.GetDateTime(3).Date.Year, rdr.GetDateTime(3).Date.Month, rdr.GetDateTime(3).Date.Day);
                String team_id_string = d.ToString("yyyy-MM-dd");

                Empleado prov = new Empleado(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), team_id_string, rdr.GetInt32(4), rdr.GetString(5), rdr.GetString(6), rdr.GetInt32(7), rdr.GetInt32(8));
            lista_empleados.Add(prov);
        }
        rdr.Close();
        lvEmpleados.ItemsSource = lista_empleados;
    }

        private void btnDelCli_Click(object sender, RoutedEventArgs e)
        {
            int index = lvEmpleados.SelectedIndex;
            int id_borrar = lista_empleados[index].id;
            string query = $"DELETE FROM empleados WHERE id={id_borrar};";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();
            cargar_empleados();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
             if (String.IsNullOrEmpty(lb_id.Text))
                {
                int id = crear_id();
               
                int municipi = (cb_municipi.SelectedItem as Municipio).ID;
                int provincia = (cb_provincia.SelectedItem as Provincia).ID;
                string query = $"insert into empleados (id,nombre,apellidos,fecha_nacimiento,telefono,email,direccion,codi_municipi,codi_provincia) values('{id+1}','{ tb_nom.Text} ','{tb_apellido.Text}','{tb_nacimiento.Text}','{tb_telefono.Text}','{tb_email.Text}','{tb_direccion.Text}','{municipi}','{ provincia }');";
                MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
            }
           else
            {
                int municipi = (cb_municipi.SelectedItem as Municipio).ID;
                int provincia = (cb_provincia.SelectedItem as Provincia).ID;
                String id = lb_id.Text;
                string query = $"UPDATE empleados set id='{id}',nombre='{tb_nom.Text}',apellidos='{tb_apellido.Text}',fecha_nacimiento='{tb_nacimiento.Text}',telefono='{tb_telefono.Text}',email='{tb_email.Text}',direccion='{tb_direccion.Text}',codi_municipi='{municipi}',codi_provincia='{provincia}' WHERE id='{id }';";
                MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
                cargar_empleados();
            }
            cargar_empleados();


            lb_id.Text = "";
            tb_apellido.Text = "";
            tb_direccion.Text = "";
            tb_email.Text = "";
            tb_nacimiento.Text = "";
            tb_telefono.Text = "";
            tb_nom.Text = "";
            cb_municipi.SelectedItem = null;
            cb_provincia.SelectedItem = null;
        }
        private void cargar_provincia()
        {
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
            cb_provincia.ItemsSource = provincias;
            connexio.Close();
            Console.WriteLine(cb_provincia.SelectedItem);
        }
        private void onSelectProvincia(object sender, SelectionChangedEventArgs e)
        {
            if(cb_provincia.SelectedItem == null)
            {
                cb_municipi.SelectedItem = null;
            }
            else
            {
                cb_municipi.ItemsSource =
                           ((sender as ComboBox).SelectedItem as Provincia).getMunicipios();
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Empleado modificar = lvEmpleados.SelectedItem as Empleado;

            if (modificar != null)
            {
                lb_id.Text = modificar.id.ToString();
                tb_nom.Text = modificar.nom.ToString();
                tb_apellido.Text = modificar.apellidos.ToString();
                tb_direccion.Text = modificar.direccion.ToString();
                tb_email.Text = modificar.email.ToString();
                tb_nacimiento.Text = modificar.fecha.ToString();
                tb_telefono.Text = modificar.telefono.ToString();
                cb_provincia.Text = modificar.codi_provincia.ToString();
                cb_municipi.Text = modificar.codi_municipi.ToString();


                ItemCollection icP = cb_provincia.Items;
                for (int i = 0; i < icP.Count; i++)
                {
                    if ((icP[i] as Provincia).ID == modificar.codi_provincia)
                    {
                        cb_provincia.SelectedItem = icP[i];
                        break;
                    }
                }

                ItemCollection icM = cb_municipi.Items;
                for (int i = 0; i < icM.Count; i++)
                {
                    if ((icM[i] as Municipio).ID == modificar.codi_municipi)
                    {
                        cb_municipi.SelectedItem = icM[i];
                        break;

                    }
                    else
                    {
                        MessageBox.Show("Selecciona un empleado");

                    }


                }
            }
        }

        private int crear_id()
        {
            string query = $"select max(id) from empleados;";
            MySqlCommand cmd = new MySqlCommand(query, connexio.Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            int id = 0;

            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
            }

            rdr.Close();
            return id;
        }



    }

}
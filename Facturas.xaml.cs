using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para Factura.xaml
    /// </summary>
    public partial class Facturas : Page
    {
        public ObservableCollection<Factura> FacturasList { get; } = new();
         public ObservableCollection<Cliente> ClientesList { get; } = new();
        public ObservableCollection<Empleado> EmpleadosList { get; } = new();

        private List<Factura> FacturasListOriginal = new();

        private MySqlConnection Conn = new Connexio().Conn;

        public Facturas()
        {
            InitializeComponent();
            this.DataContext = this;
            cargar();
        }

        private void onInsertar(object sender, RoutedEventArgs e)
        {
        }

        public void cargar()
        {
            FacturasList.Clear();
            FacturasListOriginal.Clear();
            ClientesList.Clear();
            EmpleadosList.Clear();
            string query = "select * from facturas;";
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Factura prov = new Factura(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetDateTime(3), rdr.GetFloat(4));
                FacturasList.Add(prov);
                FacturasListOriginal.Add(prov);
            }
            rdr.Close();

            query = "select * from clientes;";
            cmd = new MySqlCommand(query, Conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime d = new DateTime(rdr.GetDateTime(4).Date.Year, rdr.GetDateTime(4).Date.Month, rdr.GetDateTime(4).Date.Day);
                Cliente prov = new Cliente(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), d, rdr.GetInt32(5), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetInt32(9));
                ClientesList.Add(prov);
            }
            rdr.Close();

            query = "select * from empleados;";
             cmd = new MySqlCommand(query, Conn);
             rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime d = new DateTime(rdr.GetDateTime(3).Date.Year, rdr.GetDateTime(3).Date.Month, rdr.GetDateTime(3).Date.Day);
                Empleado prov = new Empleado(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), d.ToString(), rdr.GetInt32(4), rdr.GetString(5), rdr.GetString(6), rdr.GetInt32(7), rdr.GetInt32(8));
                EmpleadosList.Add(prov);
            }
            rdr.Close();

            IsInsert.IsEnabled = true;
            IsDelete.IsEnabled = true;
            IsUpdate.IsEnabled = true;

            if (IsInsert.IsChecked != null && !(bool)IsInsert.IsChecked
                && IsDelete.IsChecked != null && !(bool)IsDelete.IsChecked
                && IsUpdate.IsChecked != null && !(bool)IsUpdate.IsChecked)
            {
                IsInsert.IsChecked = true;
            }


            RadioButtonsUpdated();
        }

        private void RadioButtonsUpdated()
        {
            ComboBoxFacturas.SelectedItem = null;
            ComboBoxClientes.SelectedItem = null;
            ComboBoxEmpleados.SelectedItem = null;
            DatePickerPublDate.Text = "";
            TextBoxPreuFinal.Text = "";

            Apply.IsEnabled = true;
            Load.IsEnabled = true;
            Save.IsEnabled = true;

            if (IsInsert.IsChecked != null && (bool)IsInsert.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = false;
                ComboBoxClientes.IsEnabled = true;
                ComboBoxEmpleados.IsEnabled = true;
                DatePickerPublDate.IsEnabled = true;
                TextBoxPreuFinal.IsEnabled = true;
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = true;
                ComboBoxClientes.IsEnabled = false;
                ComboBoxEmpleados.IsEnabled = false;
                DatePickerPublDate.IsEnabled = false;
                TextBoxPreuFinal.IsEnabled = false;
            }
            else if (IsUpdate.IsChecked != null && (bool)IsUpdate.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = true;
                ComboBoxClientes.IsEnabled = false;
                ComboBoxEmpleados.IsEnabled = false;
                DatePickerPublDate.IsEnabled = true;
                TextBoxPreuFinal.IsEnabled = false;
            }
            else
            {
                ComboBoxFacturas.IsEnabled = false;
                ComboBoxClientes.IsEnabled = false;
                ComboBoxEmpleados.IsEnabled = false;
                DatePickerPublDate.IsEnabled = false;
                TextBoxPreuFinal.IsEnabled = false;
                Apply.IsEnabled = false;
                Save.IsEnabled = false;
            }
        }

        private void IsInsert_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonsUpdated();
        }

        private void IsDelete_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonsUpdated();
        }

        private void IsUpdate_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonsUpdated();
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            if (IsInsert.IsChecked != null && (bool)IsInsert.IsChecked)
            {
                try
                {
                    int id = 0;
                    foreach (Factura factura in FacturasList)
                    {
                        id = factura.id > id ? factura.id : id;
                    }
                    foreach (Factura factura in FacturasListOriginal)
                    {
                        id = factura.id > id ? factura.id : id;
                    }
                    id++;

                    DateTime fecha = DateTime.MinValue;
                    float precio = 0.0f;
                    int stock = 0;
                    Cliente cl = new Cliente(0, "", "", "", fecha, 0, "","",0,0);
                    Empleado em = new Empleado(0,"", "", fecha.ToString(), 0,"","",0,0);
                    try
                    {
                        fecha = (DateTime)DatePickerPublDate.SelectedDate;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("La fecha no tiene un formato válido.");
                    }
                    try
                    {
                        precio = float.Parse(TextBoxPreuFinal.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El precio debe ser un número con decimales.");
                    }
                    try
                    {
                        cl = (Cliente)ComboBoxClientes.SelectedItem;
                        if (cl == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse un cliente.");
                    }
                    try
                    {
                        em = (Empleado)ComboBoxEmpleados.SelectedItem;
                        if (em == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse un empleado.");
                    }

                    FacturasList.Add(new Factura(id, cl.id, em.id, fecha, precio));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al insertar");
                }
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                if (ComboBoxFacturas.SelectedItem == null)
                {
                    MessageBox.Show("No se ha seleccionado ningún registro", "Error al eliminar");
                }
                else
                {
                    try
                    {
                        FacturasList.Remove((Factura)ComboBoxFacturas.SelectedItem);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error al eliminar");
                    }
                }

            }
            else if (IsUpdate.IsChecked != null && (bool)IsUpdate.IsChecked)
            {
                try
                {
                    Factura factura;
                    try
                    {
                        factura = (Factura)ComboBoxFacturas.SelectedItem;
                        if (factura == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse una factura.");
                    }

                    DateTime fecha = DateTime.MinValue;
                    try
                    {
                        fecha = (DateTime)DatePickerPublDate.SelectedDate;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("La fecha no tiene un formato válido.");
                    }
                    
                    factura.fecha = fecha;

                    FacturasList.Insert(ComboBoxFacturas.SelectedIndex, factura);
                    FacturasList.RemoveAt(ComboBoxFacturas.SelectedIndex);
                    ComboBoxFacturas.SelectedItem = factura;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al actualizar");
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            cargar();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction transaction = null;
            try
            {
                transaction = Conn.BeginTransaction();
                foreach (Factura facturaOriginal in FacturasListOriginal)
                {
                    bool found = false;
                    foreach (Factura facturaNueva in FacturasList)
                    {
                        if (facturaOriginal.id == facturaNueva.id)
                        {
                            //antes existía, ahora también, lo actualizamos
                            String query = "UPDATE facturas SET "
                                + "id = '" + facturaNueva.id + "', "
                                + "cliente_id = '" + facturaNueva.cliente + "', "
                                + "empleados_id = '" + facturaNueva.empleado + "', "
                                + "fecha = '" + DateToString(facturaNueva.fecha) + "', "
                                + "precio_final = '" + SanitazeFloat(facturaNueva.preciofinal) + "'"
                                + " WHERE id = '" + facturaOriginal.id + "';";
                            MySqlCommand cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        //antes existía, ahora no, lo borramos
                        String query = "DELETE FROM facturas WHERE id = '" + facturaOriginal.id + "';";
                        MySqlCommand cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                foreach (Factura facturaNueva in FacturasList)
                {
                    bool found = false;
                    foreach (Factura facturaOriginal in FacturasListOriginal)
                    {
                        if (facturaOriginal.id == facturaNueva.id)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        String query = "INSERT INTO facturas VALUES("
                            + "'" + facturaNueva.id + "', "
                            + "'" + facturaNueva.cliente + "', "
                            + "'" + facturaNueva.empleado + "', "
                            + "'" + DateToString(facturaNueva.fecha) + "', "
                            + "'" + SanitazeFloat(facturaNueva.preciofinal)
                            + "');";
                        MySqlCommand cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                cargar();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                MessageBox.Show(ex.Message, "Error al guardar en BDD");
            }
        }

        private static string SanitazeFloat(float f)
        {
            return f.ToString().Replace(",", ".");
        }
        private static string DateToString(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }
    }
}

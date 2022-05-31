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
    /// Lógica de interacción para LiniaFactura.xaml
    /// </summary>
    public partial class LiniaFactura : Page
    {
        public ObservableCollection<LinFacturas> lista_LinFacturas { get; } = new();
        public ObservableCollection<Libro> listaLibros { get; } = new();

        List<Factura> listaFacturas = new List<Factura>();

        List<Editorial> EditorialList = new List<Editorial>();

        List<LinFacturas> lista_LinFacturasOriginal = new List<LinFacturas>();

        private MySqlConnection Conn = new Connexio().Conn;
        public LiniaFactura()
        {
            InitializeComponent();
            this.DataContext = this;
            cargar();
        }
        public void cargar()
        {
            lista_LinFacturas.Clear();
            lista_LinFacturasOriginal.Clear();
            listaFacturas.Clear();
            listaLibros.Clear();

            string query = "select * from liniea_factura;";
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                LinFacturas prov = new LinFacturas(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetFloat(3));
                lista_LinFacturas.Add(prov);
                lista_LinFacturasOriginal.Add(prov);
            }
            rdr.Close();


            query = "SELECT id, cif, nombre, telefono, direccion, codi_postal, codi_provincia, codi_municipi, email FROM editoriales;";
            cmd = new MySqlCommand(query, Conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                EditorialList.Add(new Editorial(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetString(8)));
            }
            rdr.Close();

            query = "SELECT editorial_id, id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma FROM libros;";
            cmd = new MySqlCommand(query, Conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int EditorialID = rdr.GetInt32(0);
                foreach (Editorial ed in EditorialList)
                {
                    if (ed.id == EditorialID)
                    {
                        int id = rdr.GetInt32(1);

                        string isbn = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                        string titulo = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                        string autor = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                        DateTime fecha_publicacion = rdr.IsDBNull(5) ? DateTime.MinValue : rdr.GetDateTime(5);
                        string genero = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                        double precio = rdr.IsDBNull(7) ? 0.0 : rdr.GetDouble(7);
                        int stock = rdr.IsDBNull(8) ? 0 : rdr.GetInt32(8);
                        string idioma = rdr.IsDBNull(9) ? "" : rdr.GetString(9);

                        listaLibros.Add(new Libro(id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma, ed));
                        break;
                    }
                }
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
            ComboBoxLibros.SelectedItem = null;
            TextBoxPreuPerUnitat.Text = "";
            TextQuantitat.Text = "";

            Apply.IsEnabled = true;
            Load.IsEnabled = true;
            Save.IsEnabled = true;

            if (IsInsert.IsChecked != null && (bool)IsInsert.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = true;
                ComboBoxLibros.IsEnabled = true;
                TextBoxPreuPerUnitat.IsEnabled = true;
                TextQuantitat.IsEnabled = true;
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = true;
                ComboBoxLibros.IsEnabled = true;
                TextBoxPreuPerUnitat.IsEnabled = false;
                TextQuantitat.IsEnabled = false;
            }
            else if (IsUpdate.IsChecked != null && (bool)IsUpdate.IsChecked)
            {
                ComboBoxFacturas.IsEnabled = false;
                ComboBoxLibros.IsEnabled = false;
                TextBoxPreuPerUnitat.IsEnabled = true;
                TextQuantitat.IsEnabled = true;
            }
            else
            {
                ComboBoxFacturas.IsEnabled = false;
                ComboBoxLibros.IsEnabled = false;
                TextBoxPreuPerUnitat.IsEnabled = false;
                TextQuantitat.IsEnabled = false;
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

                    float precio = 0.0f;
                    int stock = 0;
                    Factura fc = new Factura(0, 0,0, new DateTime(), 0f);
                    Editorial ed = new Editorial(0, "", "", "", "", 0, 0, 0, "");
                    Libro lb = new Libro(0, "", "", "", new DateTime(), "", 0.00, 0, "", ed);
                    try
                    {
                        precio = float.Parse(TextBoxPreuPerUnitat.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El precio debe ser un número con decimales.");
                    }
                    try
                    {
                        fc = (Factura)ComboBoxFacturas.SelectedItem;
                        if (fc == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse una factura.");
                    }
                    try
                    {
                        lb = (Libro)ComboBoxLibros.SelectedItem;
                        if (lb == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse un libro.");
                    }

                    lista_LinFacturas.Add(new LinFacturas(fc.id, lb.ID, stock, precio));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al insertar");
                }
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                if(ComboBoxLibros.SelectedItem != null && ComboBoxFacturas.SelectedItem != null)
                {
                   


                }
                else
                {
                    MessageBox.Show("No se ha seleccionado ningún registro", "Error al eliminar");
                }

                if (ComboBoxFacturas.SelectedItem == null)
                {
                    foreach(LinFacturas lf in lista_LinFacturas)
                    {

                    }
                }
                else
                {
                    try
                    {
                        lista_LinFacturas.Remove((LinFacturas)ComboBoxFacturas.SelectedItem);
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
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("La fecha no tiene un formato válido.");
                    }

                    factura.fecha = fecha;


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
        }

        private static string SanitazeFloat(float f)
        {
            return f.ToString().Replace(",", ".");
        }
        private static string DateToString(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }

        public void cargar2()
        {
            string query = "select* from facturas;";
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                LinFacturas prov = new LinFacturas(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetFloat(3));
                lista_LinFacturas.Add(prov);
            }
            rdr.Close();
            lvLiniaFacturas.ItemsSource = lista_LinFacturas;
        }
    }
}

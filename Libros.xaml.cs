using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Interaction logic for Libros.xaml
    /// </summary>
    public partial class Libros : Window
    {
        public ObservableCollection<Libro> LibrosList { get; } = new();
        public ObservableCollection<Editorial> EditorialList { get; } = new();
        private List<Libro> LibrosListOriginal = new();
        private MySqlConnection Conn = new Connexio().Conn;
        public Libros()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadDB();
        }

        private void LoadDB()
        {
            LibrosList.Clear();
            LibrosListOriginal.Clear();
            EditorialList.Clear();

            String query = "SELECT id, cif, nombre, telefono, direccion, codi_postal, codi_provincia, codi_municipi, email FROM editoriales;";
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
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

                        LibrosList.Add(new Libro(id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma, ed));
                        LibrosListOriginal.Add(new Libro(id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma, ed));
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
            Apply.IsEnabled = true;
            Load.IsEnabled = true;
            Save.IsEnabled = true;
            if (IsInsert.IsChecked != null && (bool) IsInsert.IsChecked)
            {
                ComboBoxLibros.IsEnabled = false;
                TextBoxISBN.IsEnabled = true;
                TextBoxTitle.IsEnabled = true;
                TextBoxAuthor.IsEnabled = true;
                DatePickerPublDate.IsEnabled = true;
                TextBoxGenre.IsEnabled = true;
                TextBoxPrice.IsEnabled = true;
                TextBoxStock.IsEnabled = true;
                TextBoxLang.IsEnabled = true;
                ComboBoxEditorial.IsEnabled = true;
            } else if (IsDelete.IsChecked != null && (bool) IsDelete.IsChecked)
            {
                ComboBoxLibros.IsEnabled = true;
                TextBoxISBN.IsEnabled = false;
                TextBoxTitle.IsEnabled = false;
                TextBoxAuthor.IsEnabled = false;
                DatePickerPublDate.IsEnabled = false;
                TextBoxGenre.IsEnabled = false;
                TextBoxPrice.IsEnabled = false;
                TextBoxStock.IsEnabled = false;
                TextBoxLang.IsEnabled = false;
                ComboBoxEditorial.IsEnabled = false;
            } else if (IsUpdate.IsChecked != null && (bool)IsUpdate.IsChecked)
            {
                ComboBoxLibros.IsEnabled = true;
                TextBoxISBN.IsEnabled = true;
                TextBoxTitle.IsEnabled = true;
                TextBoxAuthor.IsEnabled = true;
                DatePickerPublDate.IsEnabled = true;
                TextBoxGenre.IsEnabled = true;
                TextBoxPrice.IsEnabled = true;
                TextBoxStock.IsEnabled = true;
                TextBoxLang.IsEnabled = true;
                ComboBoxEditorial.IsEnabled = true;
            } else
            {
                ComboBoxLibros.IsEnabled = false;
                TextBoxISBN.IsEnabled = false;
                TextBoxTitle.IsEnabled = false;
                TextBoxAuthor.IsEnabled = false;
                DatePickerPublDate.IsEnabled = false;
                TextBoxGenre.IsEnabled = false;
                TextBoxPrice.IsEnabled = false;
                TextBoxStock.IsEnabled = false;
                TextBoxLang.IsEnabled = false;
                ComboBoxEditorial.IsEnabled = false;
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
                int id = 0;

                string isbn = TextBoxISBN.Text;
                string titulo = TextBoxTitle.Text;
                string autor = TextBoxAuthor.Text;
                DateTime fecha_publicacion = (DateTime) DatePickerPublDate.SelectedDate;
                string genero = TextBoxGenre.Text;
                double precio = Double.Parse(TextBoxPrice.Text);
                int stock = Int32.Parse(TextBoxStock.Text);
                string idioma = TextBoxLang.Text;
                Editorial ed = (Editorial) ComboBoxEditorial.SelectedItem;

                LibrosList.Add(new Libro(id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma, ed));
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                if (ComboBoxLibros.SelectedItem != null)
                {
                    LibrosList.Remove((Libro) ComboBoxLibros.SelectedItem);
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

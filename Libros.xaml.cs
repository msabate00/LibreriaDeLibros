using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Controls;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Interaction logic for Libros.xaml
    /// </summary>
    public partial class Libros : Page
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
            ComboBoxLibros.SelectedItem = null;
            TextBoxISBN.Text = "";
            TextBoxTitle.Text = "";
            TextBoxAuthor.Text = "";
            DatePickerPublDate.Text = "";
            TextBoxGenre.Text = "";
            TextBoxPrice.Text = "";
            TextBoxStock.Text = "";
            TextBoxLang.Text = "";
            ComboBoxEditorial.SelectedItem = null;

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
                try
                {
                    int id = 0;
                    foreach (Libro libro in LibrosList)
                    {
                        id = libro.ID > id ? libro.ID : id;
                    }
                    foreach (Libro libro in LibrosListOriginal)
                    {
                        id = libro.ID > id ? libro.ID : id;
                    }
                    id++;

                    string isbn = TextBoxISBN.Text;
                    string titulo = TextBoxTitle.Text;
                    string autor = TextBoxAuthor.Text;
                    string genero = TextBoxGenre.Text;
                    string idioma = TextBoxLang.Text;

                    DateTime fecha_publicacion = DateTime.MinValue;
                    double precio = 0.0;
                    int stock = 0;
                    Editorial ed = new Editorial(0, "", "", "", "", 0, 0, 0, "");
                    try
                    {
                        fecha_publicacion = (DateTime)DatePickerPublDate.SelectedDate;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("La fecha no tiene un formato válido.");
                    }
                    try
                    {
                        precio = Double.Parse(TextBoxPrice.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El precio debe ser un número con decimales.");
                    }
                    try
                    {
                        stock = Int32.Parse(TextBoxStock.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El stock debe ser un número entero.");
                    }
                    try
                    {
                        ed = (Editorial)ComboBoxEditorial.SelectedItem;
                        if (ed == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse una editorial.");
                    }

                    LibrosList.Add(new Libro(id, isbn, titulo, autor, fecha_publicacion, genero, precio, stock, idioma, ed));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al insertar");
                }
            }
            else if (IsDelete.IsChecked != null && (bool)IsDelete.IsChecked)
            {
                if (ComboBoxLibros.SelectedItem == null)
                {
                    MessageBox.Show("No se ha seleccionado ningún registro", "Error al eliminar");
                }
                else {
                    try
                    {
                        LibrosList.Remove((Libro)ComboBoxLibros.SelectedItem);
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
                    Libro libro = new Libro(0, "", "", "", DateTime.MinValue, "", 0, 0, "", new Editorial(0, "", "", "", "", 0, 0, 0, ""));
                    try
                    {
                        libro = (Libro)ComboBoxLibros.SelectedItem;
                        if (libro == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse un libro.");
                    }

                    string isbn = TextBoxISBN.Text;
                    string titulo = TextBoxTitle.Text;
                    string autor = TextBoxAuthor.Text;
                    string genero = TextBoxGenre.Text;
                    string idioma = TextBoxLang.Text;

                    DateTime fecha_publicacion = DateTime.MinValue;
                    double precio = 0.0;
                    int stock = 0;
                    Editorial ed = new Editorial(0, "", "", "", "", 0, 0, 0, "");
                    try
                    {
                        fecha_publicacion = (DateTime)DatePickerPublDate.SelectedDate;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("La fecha no tiene un formato válido.");
                    }
                    try
                    {
                        precio = Double.Parse(TextBoxPrice.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El precio debe ser un número con decimales.");
                    }
                    try
                    {
                        stock = Int32.Parse(TextBoxStock.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("El stock debe ser un número entero.");
                    }
                    try
                    {
                        ed = (Editorial)ComboBoxEditorial.SelectedItem;
                        if (ed == null)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Debe seleccionarse una editorial.");
                    }

                    libro.ISBN = isbn;
                    libro.Titulo = titulo;
                    libro.Autor = autor;
                    libro.Fecha = fecha_publicacion;
                    libro.Genero = genero;
                    libro.Precio = precio;
                    libro.Stock = stock;
                    libro.Idioma = idioma;
                    libro.FKEditorial = ed;

                    LibrosList.Insert(ComboBoxLibros.SelectedIndex, libro);
                    LibrosList.RemoveAt(ComboBoxLibros.SelectedIndex);
                    ComboBoxLibros.SelectedItem = libro;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al actualizar");
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MySqlTransaction transaction = null;
            try
            {
                transaction = Conn.BeginTransaction();
                foreach (Libro libroOriginal in LibrosListOriginal)
                {
                    bool found = false;
                    foreach (Libro libroNuevo in LibrosList)
                    {
                        if (libroOriginal.ID == libroNuevo.ID)
                        {
                            //antes existía, ahora también, lo actualizamos
                            String query = "UPDATE libros SET "
                                + "id = '" + libroNuevo.ID + "', "
                                + "editorial_id = '" + libroNuevo.FKEditorial.id + "', "
                                + "isbn = '" + libroNuevo.ISBN + "', "
                                + "titulo = '" + libroNuevo.Titulo + "', "
                                + "autor = '" + libroNuevo.Autor + "', "
                                + "fecha_publicacion = '" + DateToString(libroNuevo.Fecha) + "', "
                                + "genero = '" + libroNuevo.Genero + "', "
                                + "precio = '" + SanitazeDouble(libroNuevo.Precio) + "', "
                                + "stock = '" + libroNuevo.Stock + "', "
                                + "idioma = '" + libroNuevo.Idioma + "' "
                                + "WHERE id = '" + libroOriginal.ID + "';";
                            MySqlCommand cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        //antes existía, ahora no, lo borramos
                        String query = "DELETE FROM libros WHERE id = " + libroOriginal.ID + ";";
                        MySqlCommand cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                foreach (Libro libroNuevo in LibrosList)
                {
                    bool found = false;
                    foreach (Libro libroOriginal in LibrosListOriginal)
                    {
                        if (libroOriginal.ID == libroNuevo.ID)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        String query = "INSERT INTO libros VALUES("
                            + "'" + libroNuevo.ID + "', "
                            + "'" + libroNuevo.FKEditorial.id + "', "
                            + "'" + libroNuevo.ISBN + "', "
                            + "'" + libroNuevo.Titulo + "', "
                            + "'" + libroNuevo.Autor + "', "
                            + "'" + DateToString(libroNuevo.Fecha) + "', "
                            + "'" + libroNuevo.Genero + "', "
                            + "'" + SanitazeDouble(libroNuevo.Precio) + "', "
                            + "'" + libroNuevo.Stock + "', "
                            + "'" + libroNuevo.Idioma + "'"
                            + ");";
                        MySqlCommand cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                LoadDB();
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

        private static string SanitazeDouble(Double d)
        {
            return d.ToString().Replace(",", ".");
        }
        private static string DateToString(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }
    }
}

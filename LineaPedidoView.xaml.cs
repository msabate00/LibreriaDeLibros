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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Lógica de interacción para LineaPedido.xaml
    /// </summary>
    public partial class LineaPedidoView : Window
    {
        PedidoDao pedidoDao = new PedidoDao();
        ObservableCollection<LineaPedido> lista_lineas = new ObservableCollection<LineaPedido>();
        List<LineaPedido> nuevas_lineas = new List<LineaPedido>();
        List<Editorial> editorialesList = new List<Editorial>();
        List<Libro> librosList = new List<Libro>();
        int totalArticles;
        double totalPrecio;
        public Pedido? pedidoModificar;

        public LineaPedidoView(Pedido p)
        {
            InitializeComponent();
            pedidoModificar = p;
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public LineaPedidoView()
        {
            InitializeComponent();
            try
            {
                valoresDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void loadCombo()
        {
            editorialesList = pedidoDao.GetEditoriales();
            librosList = pedidoDao.GetLibros();
            cbEditorials.ItemsSource = editorialesList;
            cbLibrosList.ItemsSource = librosList;
        }

        public void valoresDefault()
        {
            loadCombo();
            dpFechaPedido.SelectedDate = DateTime.Now;
            dpFechaEntrega.SelectedDate = DateTime.Now.AddDays(10);
            tbUnidades.Text = "5";
            tbPrecioUnidades.Text = "2.5";
        }

        public void load()
        {
            loadCombo();
            cbEditorials.IsEnabled = false;
            refreshLineasPedido();
            dpFechaPedido.SelectedDate = pedidoModificar.FechaPedido;
            dpFechaEntrega.SelectedDate = pedidoModificar.FechaEntrega;
        }

        public void refreshLineasPedido()
        {
            lista_lineas = pedidoDao.GetLineasByPedidoId(pedidoModificar);
            foreach (var linea in lista_lineas)
            {
                linea.LibroNom = pedidoDao.GetLibroNameById(linea.LibroId);
            }
            GetTotals();
            lvlineaP.ItemsSource = lista_lineas;

        }

        public void GetTotals()
        {
            totalArticles = 0;
            totalPrecio = 0;

            if (lista_lineas.Count > 0)
            {
                foreach (var linea in lista_lineas)
                {
                    totalArticles += linea.Cantidad;
                    totalPrecio += linea.Precio * linea.Cantidad;
                }
            }
            else
            {
                totalPrecio = 0;
                totalArticles = 0;
            }
            tbTotalArticles.Text = totalArticles.ToString();
            tbTotalPrecio.Text = totalPrecio.ToString();
        }

        private void onLimpiarClick(object sender, RoutedEventArgs e)
        {
            cbEditorials.SelectedIndex = 0;
            dpFechaPedido.SelectedDate = DateTime.Now;
            dpFechaEntrega.SelectedDate = DateTime.Now.AddDays(10);
            cbLibrosList.SelectedIndex = 0;
            tbUnidades.Text = "";
            tbPrecioUnidades.Text = "";

        }

        private void onAgregarClick(object sender, RoutedEventArgs e)
        {
            int nroLineas = lista_lineas.Count;
            int counter = nroLineas + 1;
            LineaPedido lp = new LineaPedido();

            if (cbEditorials.SelectedItem == null && pedidoModificar == null)
            {
                MessageBox.Show("Selecciona una editorial.");
            }
            else
            {
                if (cbLibrosList.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona una libro.");
                }
                else
                {
                    Libro libro = cbLibrosList.SelectedItem as Libro;
                    if (lista_lineas.Any(x => x.LibroNom.Equals(libro.Titulo)))
                    {
                        MessageBox.Show("Este libro ya existe en las líneas de pedido.");
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(tbUnidades.Text))
                        {
                            MessageBox.Show("Selecciona un número de unidades.");
                        }
                        else
                        {
                            try
                            {
                                int units = Convert.ToInt32(tbUnidades.Text);
                                if (units > 0)
                                {
                                    lp.Counter = counter;
                                    lp.LibroId = (cbLibrosList.SelectedItem as Libro).ID;
                                    lp.LibroNom = pedidoDao.GetLibroNameById(lp.LibroId);
                                    lp.Cantidad = units;
                                    if (String.IsNullOrEmpty(tbPrecioUnidades.Text))
                                    {
                                        lp.Precio = 2.5;
                                    }
                                    else
                                    {
                                        double precioUnits = Convert.ToDouble(tbPrecioUnidades.Text);
                                        if (precioUnits > 0)
                                        {
                                            lp.Precio = precioUnits;
                                        }
                                        else { MessageBox.Show("Precio unidades debe ser > 0"); lp.Precio = 1; }
                                    }
                                    nuevas_lineas.Add(lp);
                                    lista_lineas.Add(lp);
                                    lvlineaP.ItemsSource = lista_lineas;
                                    GetTotals();

                                    //update de pedido
                                    if (pedidoModificar != null)
                                    {
                                        pedidoDao.insertarLineaPedidoWithPedidoId(pedidoModificar.Id, lp);
                                    }
                                }
                                else { MessageBox.Show("Unidades debe ser > 0"); }
                            }
                            catch (FormatException ex)
                            {
                                MessageBox.Show("Error de Formato, insertar valores numéricos.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message.ToString());
                            }
                        }
                    }
                }
            }

        }

        private void onInsertarClick(object sender, RoutedEventArgs e)
        {
            //Si no hay lineas de pedido
            if (lista_lineas.Count == 0)
            {
                MessageBox.Show("Agrega líneas de pedido!");
            }
            else
            {
                //Si es insert
                if (pedidoModificar == null)
                {
                    Pedido pedidoNuevo = new Pedido
                    {
                        EditorialId = (cbEditorials.SelectedIndex) + 1,
                        FechaPedido = dpFechaPedido.SelectedDate.Value,
                        FechaEntrega = dpFechaEntrega.SelectedDate.Value,
                        Cantidad = totalArticles,
                        PrecioFinal = totalPrecio
                    };
                    try
                    {
                        pedidoDao.insertarPedido(pedidoNuevo);
                        int pedidoId = pedidoDao.GetMaxIdPedido();
                        foreach (var linea in nuevas_lineas)
                        {
                            pedidoDao.insertarLineaPedido(pedidoId, linea);
                        }
                        MessageBox.Show("Pedido añadido");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }

                //Si es update
                else
                {
                    Pedido pedidoUpdate = new Pedido();
                    pedidoUpdate.Id = pedidoModificar.Id;
                    pedidoUpdate.EditorialId = pedidoModificar.EditorialId;
                    pedidoUpdate.FechaPedido = dpFechaPedido.SelectedDate.Value;
                    pedidoUpdate.FechaEntrega = dpFechaEntrega.SelectedDate.Value;
                    pedidoUpdate.Cantidad = totalArticles;
                    pedidoUpdate.PrecioFinal = totalPrecio;
                    try
                    {
                        pedidoDao.updatePedido(pedidoUpdate);
                        MessageBox.Show("Pedido Actualizado");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }


                }
            }
        }
        private void onEliminarClick(object sender, RoutedEventArgs e)
        {
            if (lvlineaP.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una linea de pedido.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Seguro que quieres eliminar la linea de pedido?",
                "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //Si es un update de pedido
                    if (pedidoModificar != null)
                    {
                        try
                        {
                            pedidoDao.eliminarLineaPedidoByIdPedidoAndIdLibro(pedidoModificar.Id, (lvlineaP.SelectedItem as LineaPedido).LibroId);
                            refreshLineasPedido();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        nuevas_lineas.Remove(lvlineaP.SelectedItem as LineaPedido);
                        lista_lineas.Remove(lvlineaP.SelectedItem as LineaPedido);
                        lvlineaP.ItemsSource = lista_lineas;
                    }


                }

            }
        }
    }
}

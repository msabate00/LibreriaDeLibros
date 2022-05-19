﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibreriaDeLibrosSL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Editoriales window = new Editoriales();
            middleFrame.Source = new Uri("Editoriales.xaml", UriKind.RelativeOrAbsolute);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Empleados window = new Empleados();
            window.Show();

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame middleFrame = rightCol;
            Libros window = new Libros();
            middleFrame.Source = new Uri("Libros.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}

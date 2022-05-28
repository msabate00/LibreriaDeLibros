using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeLibrosSL
{
    public class Libro
    {
        public int ID { get; set; }
        public Editorial FKEditorial { get; set; }
        public String ISBN { get; set; }
        public String Titulo { get; set; }
        public String Autor { get; set; }
        public DateTime Fecha { get; set; }
        public String Genero { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public String Idioma { get; set; }

        public Libro(int ID, String ISBN, String Titulo, String Autor, DateTime Fecha, String Genero, double Precio, int Stock, String Idioma, Editorial FKEditorial)
        {
            this.ID = ID;
            this.ISBN = ISBN;
            this.Titulo = Titulo;
            this.Autor = Autor;
            this.Fecha = Fecha;
            this.Genero = Genero;
            this.Precio = Precio;
            this.Stock = Stock;
            this.Idioma = Idioma;
            this.FKEditorial = FKEditorial;
        }

        public Libro()
        {

        }

        public override string ToString()
        {
            return "[" + ID + "] " + ISBN;
        }
    }
}
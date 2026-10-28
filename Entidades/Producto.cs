using System;

namespace Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; } // Propiedad para la ruta/URL

        public override string ToString()
        {
            return Nombre;
        }

    }
}
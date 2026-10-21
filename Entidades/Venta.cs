using System;
using System.Collections.Generic;

namespace Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
        public decimal Total { get; set; }

        public Venta()
        {
            Detalles = new List<DetalleVenta>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Entidades;

namespace Negocio
{
    public class VentaNegocio
    {
        private readonly ConexionGV conexion = new ConexionGV();

        // 1. Alta de Venta (Inserta en Ventas, DetalleVentas y descuenta el Stock)
        public void AltaVenta(Venta v)
        {
            if (v == null || v.Cliente == null || v.Detalles == null || v.Detalles.Count == 0)
                throw new Exception("La venta debe contener un cliente y al menos un producto.");

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // A. Insertar Cabecera de Venta
                    string queryVenta = "INSERT INTO Ventas (Fecha, IdCliente, Total) " +
                                       "VALUES (@Fecha, @IdCliente, @Total); SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdVenta = new SqlCommand(queryVenta, con, transaction);
                    cmdVenta.Parameters.AddWithValue("@Fecha", DateTime.Now);
                    cmdVenta.Parameters.AddWithValue("@IdCliente", v.Cliente.IdCliente);
                    cmdVenta.Parameters.AddWithValue("@Total", v.Total);

                    // Obtener el ID recién generado para la Venta
                    int idVentaGenerado = Convert.ToInt32(cmdVenta.ExecuteScalar());
                    v.IdVenta = idVentaGenerado;

                    // B. Insertar Detalles y Descontar Stock
                    foreach (var d in v.Detalles)
                    {
                        string queryDetalle = "INSERT INTO DetalleVentas (IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal) " +
                                              "VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario, @Subtotal)";

                        SqlCommand cmdDetalle = new SqlCommand(queryDetalle, con, transaction);
                        cmdDetalle.Parameters.AddWithValue("@IdVenta", idVentaGenerado);
                        cmdDetalle.Parameters.AddWithValue("@IdProducto", d.Producto.IdProducto);
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                        cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", d.PrecioUnitario);
                        cmdDetalle.Parameters.AddWithValue("@Subtotal", d.Subtotal);
                        cmdDetalle.ExecuteNonQuery();

                        // Descontar Stock del Producto
                        string queryStock = "UPDATE Productos SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto";
                        SqlCommand cmdStock = new SqlCommand(queryStock, con, transaction);
                        cmdStock.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                        cmdStock.Parameters.AddWithValue("@IdProducto", d.Producto.IdProducto);
                        cmdStock.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al registrar la venta en la BD: " + ex.Message);
                }
            }
        }

        // 2. Obtener todas las Ventas
        public List<Venta> ListarVentas()
        {
            List<Venta> lista = new List<Venta>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT v.IdVenta, v.Fecha, v.Total, " +
                               "c.IdCliente, c.Nombre AS NomCli, c.Apellido AS ApeCli, c.Dni " +
                               "FROM Ventas v " +
                               "INNER JOIN Clientes c ON v.IdCliente = c.IdCliente " +
                               "ORDER BY v.Fecha DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venta v = new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            Cliente = new Cliente
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Nombre = reader["NomCli"].ToString(),
                                Apellido = reader["ApeCli"].ToString(),
                                Dni = reader["Dni"].ToString()
                            }
                        };
                        lista.Add(v);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al consultar ventas desde la BD: " + ex.Message);
                }
            }

            return lista;
        }

        // 3. Buscar Ventas por Cliente
        public List<Venta> BuscarPorCliente(string texto)
        {
            texto = (texto ?? "").ToLower();
            return ListarVentas()
                .Where(v => v.Cliente != null &&
                            (v.Cliente.Nombre + " " + v.Cliente.Apellido).ToLower().Contains(texto))
                .ToList();
        }

        // 4. Buscar Ventas por Fecha
        public List<Venta> BuscarPorFecha(DateTime fecha)
        {
            return ListarVentas()
                .Where(v => v.Fecha.Date == fecha.Date)
                .ToList();
        }

        // 5. Cancelar / Anular Venta (Devuelve el Stock a SQL y borra el registro)
        public void CancelarVenta(Venta v)
        {
            if (v == null) return;

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Restablecer el Stock leyendo los detalles de la venta
                    string queryObtenerDetalles = "SELECT IdProducto, Cantidad FROM DetalleVentas WHERE IdVenta = @IdVenta";
                    SqlCommand cmdGetDetalles = new SqlCommand(queryObtenerDetalles, con, transaction);
                    cmdGetDetalles.Parameters.AddWithValue("@IdVenta", v.IdVenta);

                    List<Tuple<int, int>> productosADevolver = new List<Tuple<int, int>>();
                    using (SqlDataReader reader = cmdGetDetalles.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productosADevolver.Add(new Tuple<int, int>(
                                Convert.ToInt32(reader["IdProducto"]),
                                Convert.ToInt32(reader["Cantidad"])
                            ));
                        }
                    }

                    foreach (var item in productosADevolver)
                    {
                        string queryDevolverStock = "UPDATE Productos SET Stock = Stock + @Cantidad WHERE IdProducto = @IdProducto";
                        SqlCommand cmdStock = new SqlCommand(queryDevolverStock, con, transaction);
                        cmdStock.Parameters.AddWithValue("@Cantidad", item.Item2);
                        cmdStock.Parameters.AddWithValue("@IdProducto", item.Item1);
                        cmdStock.ExecuteNonQuery();
                    }

                    // Eliminar Detalle y Cabecera de la BD
                    string queryDelDetalle = "DELETE FROM DetalleVentas WHERE IdVenta = @IdVenta";
                    SqlCommand cmdDelDetalle = new SqlCommand(queryDelDetalle, con, transaction);
                    cmdDelDetalle.Parameters.AddWithValue("@IdVenta", v.IdVenta);
                    cmdDelDetalle.ExecuteNonQuery();

                    string queryDelVenta = "DELETE FROM Ventas WHERE IdVenta = @IdVenta";
                    SqlCommand cmdDelVenta = new SqlCommand(queryDelVenta, con, transaction);
                    cmdDelVenta.Parameters.AddWithValue("@IdVenta", v.IdVenta);
                    cmdDelVenta.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al cancelar la venta en la BD: " + ex.Message);
                }
            }
        }
    }
}
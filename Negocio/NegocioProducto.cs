using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Entidades;

namespace Negocio
{
    public class NegocioProducto
    {
        private readonly ConexionGV conexion = new ConexionGV();

        public List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT IdProducto, Nombre, Categoria, Marca, PrecioUnitario, Stock, ImagenUrl FROM Productos";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Producto p = new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            Nombre = reader["Nombre"].ToString(),
                            Categoria = reader["Categoria"].ToString(),
                            Marca = reader["Marca"].ToString(),
                            PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                            Stock = Convert.ToInt32(reader["Stock"]),
                            ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader["ImagenUrl"].ToString() : ""
                        };
                        lista.Add(p);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener productos desde la BD: " + ex.Message);
                }
            }

            return lista;
        }

        public void AltaProducto(Producto producto)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "INSERT INTO Productos (Nombre, Categoria, Marca, PrecioUnitario, Stock, ImagenUrl) " +
                               "VALUES (@Nombre, @Categoria, @Marca, @PrecioUnitario, @Stock, @ImagenUrl)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@Marca", producto.Marca);
                cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                cmd.Parameters.AddWithValue("@ImagenUrl", (object)producto.ImagenUrl ?? DBNull.Value);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al guardar producto en la BD: " + ex.Message);
                }
            }
        }

        public void ModificarProducto(Producto producto)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "UPDATE Productos SET Nombre = @Nombre, Categoria = @Categoria, Marca = @Marca, " +
                               "PrecioUnitario = @PrecioUnitario, Stock = @Stock, ImagenUrl = @ImagenUrl WHERE IdProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@Marca", producto.Marca);
                cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                cmd.Parameters.AddWithValue("@ImagenUrl", (object)producto.ImagenUrl ?? DBNull.Value);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar producto en la BD: " + ex.Message);
                }
            }
        }

        public void EliminarProducto(Producto producto)
        {
            if (producto != null)
            {
                EliminarProducto(producto.IdProducto);
            }
        }

        public void EliminarProducto(int idProducto)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Productos WHERE IdProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar producto en la BD: " + ex.Message);
                }
            }
        }

        public List<Producto> BuscarPorNombre(string nombre)
        {
            return ListarProductos()
                .Where(p => p.Nombre != null && p.Nombre.ToLower().Contains((nombre ?? "").ToLower()))
                .ToList();
        }

        public List<Producto> ProductosStockBajo(int limiteStock = 5)
        {
            return ListarProductos()
                .Where(p => p.Stock <= limiteStock)
                .ToList();
        }

        public List<Producto> OrdenarPorPrecio(bool ascendente = true)
        {
            return ascendente
                ? ListarProductos().OrderBy(p => p.PrecioUnitario).ToList()
                : ListarProductos().OrderByDescending(p => p.PrecioUnitario).ToList();
        }

        public decimal ValorInventario()
        {
            return ListarProductos().Sum(p => p.PrecioUnitario * p.Stock);
        }

        public List<Producto> FiltrarPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria) || categoria == "Todas")
                return ListarProductos();

            return ListarProductos()
                .Where(p => p.Categoria != null && p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Producto> FiltrarPorMarca(string marca)
        {
            if (string.IsNullOrWhiteSpace(marca) || marca == "Todas")
                return ListarProductos();

            return ListarProductos()
                .Where(p => p.Marca != null && p.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
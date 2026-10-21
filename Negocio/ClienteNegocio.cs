using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Entidades;

namespace Negocio
{
    public class ClienteNegocio
    {
        private readonly ConexionGV conexion = new ConexionGV();

        // 1. Obtener todos los clientes desde la Base de Datos
        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT IdCliente, Nombre, Apellido, Dni FROM Clientes";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cliente c = new Cliente
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString()
                        };
                        lista.Add(c);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar clientes desde la BD: " + ex.Message);
                }
            }

            return lista;
        }

        // 2. Dar de alta un cliente en la Base de Datos (INSERT)
        public void AltaCliente(Cliente cliente)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "INSERT INTO Clientes (Nombre, Apellido, Dni) VALUES (@Nombre, @Apellido, @Dni)";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                cmd.Parameters.AddWithValue("@Dni", cliente.Dni);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar cliente en la BD: " + ex.Message);
                }
            }
        }

        // 3. Modificar un cliente en la Base de Datos (UPDATE)
        public void ModificarCliente(Cliente cliente)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni WHERE IdCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                cmd.Parameters.AddWithValue("@Dni", cliente.Dni);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar cliente en la BD: " + ex.Message);
                }
            }
        }

        // 4. Eliminar un cliente de la Base de Datos (DELETE)
        public void EliminarCliente(Cliente cliente)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Clientes WHERE IdCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar cliente de la BD: " + ex.Message);
                }
            }
        }

        // 5. Buscar clientes por texto (Filtro SQL)
        public List<Cliente> BuscarClientes(string texto)
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                string query = "SELECT IdCliente, Nombre, Apellido, Dni FROM Clientes " +
                               "WHERE Nombre LIKE @Texto OR Apellido LIKE @Texto OR Dni LIKE @Texto";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Texto", "%" + (texto ?? "") + "%");

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Cliente c = new Cliente
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString()
                        };
                        lista.Add(c);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar clientes: " + ex.Message);
                }
            }

            return lista;
        }
    }
}
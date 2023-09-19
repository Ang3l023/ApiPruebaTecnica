using ApiPruebaTecnica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace ApiPruebaTecnica.Data
{
    public class PedidoData
    {
        public static bool Registro(string Descripcion, int IdCliente)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_INS_PEDIDOS", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally { oConexion.Close(); }
            }
        }

        public static List<Pedido> Filtrar(int? IdPedido, string? Dni)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                SqlCommand cmd = new("SP_GET_PEDIDOS", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPedido", IdPedido == null ? DBNull.Value : IdPedido);
                cmd.Parameters.AddWithValue("@Dni", Dni == null ? DBNull.Value : Dni);
                List<Pedido> pedidos = new();
                try
                {
                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            pedidos.Add(new Pedido()
                            {
                                IdPedido = (int)dr[0],
                                Descripcion = (string)dr[1]
                            });
                        }
                    }

                    return pedidos;
                }
                catch
                {
                    return pedidos;
                }
                finally { oConexion.Close(); }
            }
        }

        public static PedidoCliente Detalle(int IdPedido)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GET_PEDIDO", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPedido", IdPedido);
                PedidoCliente pedido = null;
                try
                {
                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            pedido = new PedidoCliente()
                            {
                                IdPedido = Convert.ToInt16(dr[0]),
                                Descripcion = dr[1].ToString(),
                                Nombre = dr[2].ToString(),
                                Dni = dr[3].ToString()
                            };
                        }
                    }

                    return pedido;
                }
                catch
                {
                    return null;
                }
                finally { oConexion.Close(); }
            }
        }
    }
}

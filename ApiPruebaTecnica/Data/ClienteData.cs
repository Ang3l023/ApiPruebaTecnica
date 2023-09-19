using System.Data.SqlClient;

namespace ApiPruebaTecnica.Data
{
    public class ClienteData
    {
        public static bool Registro(string Nombre, string Dni)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_INS_CLIENTE", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Dni", Dni);
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
    }
}

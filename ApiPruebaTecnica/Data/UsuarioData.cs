using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ApiPruebaTecnica.Models;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ApiPruebaTecnica.Data
{
    public class UsuarioData
    {
        public static bool Registrar(string Correo, string Clave)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_INS_USUARIO", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Clave", Clave);
                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                } catch
                {
                    return false;
                } finally { oConexion.Close(); }
            }
        }

        public static Token Login(string Correo, string Clave, string secretkey) { 
            using ( SqlConnection oConexion = new SqlConnection(Conexion.connectionString))
            {
                bool resp = false;
                SqlCommand cmd = new SqlCommand("SP_GET_LOGIN", oConexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Clave", Clave);
                SqlParameter param = cmd.Parameters.Add("@IsValid", System.Data.SqlDbType.Bit);
                param.Direction = System.Data.ParameterDirection.Output;
                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resp = (bool)param.Value;

                    if (resp == false)
                    {
                        return null;
                    }

                    var keyBytes = Encoding.ASCII.GetBytes(secretkey);

                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Correo));

                    var tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                    string tokenString = tokenHandler.WriteToken(tokenConfig);

                    Token token = new() { message = "Logeado correctamente", token = tokenString };

                    return token;
                } catch { return null; }
                finally { oConexion.Close(); }
            }
        }
    }
}

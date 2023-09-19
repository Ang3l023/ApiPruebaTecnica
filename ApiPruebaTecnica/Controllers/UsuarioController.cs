using ApiPruebaTecnica.Data;
using ApiPruebaTecnica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public string? secretkey;
        public UsuarioController(IConfiguration config)
        {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();
        }
        [HttpPost(Name = "Login")]
        public ActionResult Post([FromBody]Usuario usuario)
        {
            Token token = UsuarioData.Login(usuario.Correo, usuario.Clave, secretkey);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}

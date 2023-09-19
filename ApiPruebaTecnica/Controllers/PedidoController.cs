using ApiPruebaTecnica.Data;
using ApiPruebaTecnica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        // GET: api/<PedidoController>
        [HttpGet]
        public IEnumerable<Pedido> Get([FromQuery]int? IdPedido, [FromQuery]string? Dni)
        {
            return PedidoData.Filtrar(IdPedido, Dni);
        }

        // GET api/<PedidoController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            PedidoCliente pedido = PedidoData.Detalle(id);
            return pedido == null ? NotFound() : Ok(pedido);
        }
    }
}

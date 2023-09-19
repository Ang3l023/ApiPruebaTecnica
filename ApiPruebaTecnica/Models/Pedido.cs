namespace ApiPruebaTecnica.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Descripcion { get; set; }
        public int? IdCliente { get; set;}
    }

    public class PedidoCliente : Pedido
    {
        public string Nombre { get; set; }
        public string Dni { get; set; }
    }
}

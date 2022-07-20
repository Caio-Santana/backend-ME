#nullable disable

namespace exemploDB2.Models
{
    public partial class Pedido
    {
        public string PedidoId { get; set; }
        public string ItemDescricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
    }
}

using exemploDB2.Enum;
using System.Text.Json.Serialization;

namespace exemploDB2.Models
{
    public class StatusPedidoDto
    {
        [JsonPropertyName("status")]
        public EStatusPedido Status { get; set; }

        [JsonPropertyName("itensAprovados")]
        public int ItensAprovados { get; set; }
        
        [JsonPropertyName("valorAprovado")]
        public decimal ValorAprovado { get; set; }
        
        [JsonPropertyName("pedido")]
        public string PedidoId { get; set; }
    }
}
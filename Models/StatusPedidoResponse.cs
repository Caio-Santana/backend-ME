using System.Collections.Generic;
using exemploDB2.Enum;
using System.Text.Json.Serialization;

namespace exemploDB2.Models
{
    public class StatusPedidoResponse
    {
        [JsonPropertyName("pedido")]
        public string PedidoId { get; set; }

        [JsonPropertyName("status")]
        public IEnumerable<EStatusPedido> Status { get; set; }
    }
}
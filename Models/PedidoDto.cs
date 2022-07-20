using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace exemploDB2.Models
{
    public class PedidoDto
    {
        [JsonPropertyName("pedido")]
        public string CodigoPedido { get; set; }
        public IEnumerable<ItemDto> Itens { get; set; }
    }
}
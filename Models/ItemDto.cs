using System.Text.Json.Serialization;

namespace exemploDB2.Models
{
    public class ItemDto
    {
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("precoUnitario")]
        public decimal PrecoUnitario { get; set; }

        [JsonPropertyName("qtd")]
        public int Quantidade { get; set; }
    }
}
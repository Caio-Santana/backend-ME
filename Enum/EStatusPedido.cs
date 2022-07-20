using System.Text.Json.Serialization;

namespace exemploDB2.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EStatusPedido
    {
        CODIGO_PEDIDO_INVALIDO,
        REPROVADO,
        APROVADO,
        APROVADO_QTD_A_MENOR,
        APROVADO_VALOR_A_MENOR,
        APROVADO_QTD_A_MAIOR,
        APROVADO_VALOR_A_MAIOR
    }
}
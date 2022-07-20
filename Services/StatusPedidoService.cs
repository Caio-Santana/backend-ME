using System;
using System.Collections.Generic;
using System.Linq;
using exemploDB2.Data;
using exemploDB2.Enum;
using exemploDB2.Models;

namespace exemploDB2.Services
{
    public class StatusPedidoService
    {
        private readonly ExemploDB2Context context;

        public StatusPedidoService(ExemploDB2Context context)
        {
            this.context = context;
        }

        public StatusPedidoResponse MudancaDeStatus(StatusPedidoDto statusPedidoDto)
        {
            var statusResponse = LocalizarPedido(statusPedidoDto);
            return statusResponse;
        }

        private StatusPedidoResponse LocalizarPedido(StatusPedidoDto statusPedidoDto)
        {
            var statusResponse = new StatusPedidoResponse();
            statusResponse.PedidoId = statusPedidoDto.PedidoId;
            var listaStatus = new List<EStatusPedido>();
            statusResponse.Status = listaStatus;

            var pedido = context.Pedidos
                        .Where(p => p.PedidoId == statusPedidoDto.PedidoId)
                        .AsEnumerable();

            if (pedido is null || pedido.Count() == 0)
            {
                listaStatus.Add(EStatusPedido.CODIGO_PEDIDO_INVALIDO);
                return statusResponse;
            }

            if (statusPedidoDto.Status == EStatusPedido.REPROVADO)
            {
                listaStatus.Add(EStatusPedido.REPROVADO);
                return statusResponse;
            }

            if (statusPedidoDto.Status != EStatusPedido.APROVADO)
            {
                throw new ArgumentException();
            }

            var qtdTotalItensPedido = CalcularQtdTotalDeItensDoPedido(pedido);
            VerificarItensAprovados(listaStatus, qtdTotalItensPedido, statusPedidoDto.ItensAprovados);

            var valorTotalPedido = CalcularValorTotalDoPedido(pedido);
            VerificarValorAprovado(listaStatus, valorTotalPedido, statusPedidoDto.ValorAprovado);

            if (listaStatus.Count == 0)
            {
                listaStatus.Add(EStatusPedido.APROVADO);
            }

            return statusResponse;
        }

        private int CalcularQtdTotalDeItensDoPedido(IEnumerable<Pedido> pedido)
        {
            var quantidadeDeItens = 0;
            foreach (var item in pedido)
            {
                quantidadeDeItens += item.Quantidade;
            }
            return quantidadeDeItens;
        }

        private decimal CalcularValorTotalDoPedido(IEnumerable<Pedido> pedido)
        {
            var valorTotalDoPedido = 0m;
            foreach (var item in pedido)
            {
                valorTotalDoPedido += item.Quantidade * item.PrecoUnitario;
            }
            return valorTotalDoPedido;
        }

        private void VerificarItensAprovados(IList<EStatusPedido> status, int qtdItensPedido, int qtdItensAprovados)
        {
            if (qtdItensPedido < qtdItensAprovados)
            {
                status.Add(EStatusPedido.APROVADO_QTD_A_MAIOR);
            }
            else if (qtdItensPedido > qtdItensAprovados)
            {
                status.Add(EStatusPedido.APROVADO_QTD_A_MENOR);
            }
        }

        private void VerificarValorAprovado(IList<EStatusPedido> status, decimal valorTotalDoPedido, decimal valorTotalAprovado)
        {
            if (valorTotalDoPedido < valorTotalAprovado)
            {
                status.Add(EStatusPedido.APROVADO_VALOR_A_MAIOR);
            }
            else if (valorTotalDoPedido > valorTotalAprovado)
            {
                status.Add(EStatusPedido.APROVADO_VALOR_A_MENOR);
            }
        }
    }
}
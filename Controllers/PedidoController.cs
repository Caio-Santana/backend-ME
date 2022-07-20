using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exemploDB2.Data;
using exemploDB2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exemploDB2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private List<string> erros = new List<string>();
        private readonly ExemploDB2Context context;

        public PedidoController(ExemploDB2Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidosAsync()
        {
            var pedidos = (await context.Pedidos.ToListAsync())
                .GroupBy(
                    p => p.PedidoId,
                    p => new ItemDto
                    {
                        Descricao = p.ItemDescricao,
                        PrecoUnitario = p.PrecoUnitario,
                        Quantidade = p.Quantidade
                    });

            var listaPedidos = new List<PedidoDto>();
            foreach (var pedido in pedidos)
            {
                var pDto = new PedidoDto();
                pDto.CodigoPedido = pedido.Key;
                pDto.Itens = pedido.ToList();
                listaPedidos.Add(pDto);
            }

            return Ok(listaPedidos);
        }

        [HttpGet("{id}")]
        public ActionResult<PedidoDto> GetPedido(string id)
        {
            var pedido = context.Pedidos
                        .Where(p => p.PedidoId == id)
                        .AsEnumerable();

            if (pedido == null || pedido.Count() == 0)
            {
                return NotFound();
            }

            var pedidoDto = new PedidoDto();
            var listaItens = new List<ItemDto>();

            foreach (var item in pedido)
            {
                pedidoDto.CodigoPedido = item.PedidoId;

                var itemDto = new ItemDto();
                itemDto.Descricao = item.ItemDescricao;
                itemDto.PrecoUnitario = item.PrecoUnitario;
                itemDto.Quantidade = item.Quantidade;
                listaItens.Add(itemDto);
            }

            pedidoDto.Itens = listaItens;
            return Ok(pedidoDto);
        }

        [HttpPost()]
        public ActionResult<PedidoDto> CreatePedido(PedidoDto pedidoDto)
        {
            erros.Clear();
            if (string.IsNullOrWhiteSpace(pedidoDto.CodigoPedido))
            {
                return BadRequest("Código do pedido preenchimento obrigatório.");
            }

            var listaItens = new List<Pedido>();

            foreach (var item in pedidoDto.Itens)
            {
                ValidaItemDto(item);
                var pedido = new Pedido();
                pedido.PedidoId = pedidoDto.CodigoPedido;
                pedido.ItemDescricao = item.Descricao;
                pedido.PrecoUnitario = item.PrecoUnitario;
                pedido.Quantidade = item.Quantidade;
                listaItens.Add(pedido);
            }

            if (erros.Count > 0)
            {
                return BadRequest(erros);
            }

            try
            {
                context.Pedidos.AddRange(listaItens);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return NotFound("erro ao tentar gravar o pedido");
            }

            return Created(nameof(GetPedido), pedidoDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePedido(string id, PedidoDto pedidoDto)
        {
            erros.Clear();
            if (id != pedidoDto.CodigoPedido)
            {
                return BadRequest();
            }

            var pedidoExistente = context.Pedidos
                    .Where(p => p.PedidoId == id)
                    .AsEnumerable();

            if (pedidoExistente is null)
            {
                return NotFound();
            }

            var itensPedido = new List<Pedido>();

            foreach (var item in pedidoDto.Itens)
            {
                ValidaItemDto(item);
                var pedido = new Pedido();
                pedido.PedidoId = pedidoDto.CodigoPedido;
                pedido.ItemDescricao = item.Descricao;
                pedido.PrecoUnitario = item.PrecoUnitario;
                pedido.Quantidade = item.Quantidade;
                itensPedido.Add(pedido);
            }

            if (erros.Count > 0)
            {
                return BadRequest(erros);
            }

            try
            {
                context.Pedidos.RemoveRange(pedidoExistente);
                context.Pedidos.AddRange(itensPedido);
            }
            catch (Exception)
            {
                return NotFound("erro ao tentar atualizar o pedido");
            }

            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<PedidoDto> DeletePedido(string id)
        {
            var pedidoExistente = context.Pedidos
                    .Where(p => p.PedidoId == id)
                    .AsEnumerable();

            if (pedidoExistente is null)
            {
                return NotFound();
            }

            try
            {
                context.Pedidos.RemoveRange(pedidoExistente);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return NotFound("erro ao tentar excluir o pedido");
            }

            return NoContent();
        }

        private void ValidaItemDto(ItemDto item)
        {
            if (string.IsNullOrWhiteSpace(item.Descricao))
            {
                erros.Add("Preenchimento da descrição do item é obrigatório.");
            }

            if (item.PrecoUnitario <= 0)
            {
                erros.Add("Preço do item não pode ser menor ou igual a zero.");
            }

            if (item.Quantidade <= 0)
            {
                erros.Add("Quantidade do item não pode ser menor ou igual a zero.");
            }
        }
    }
}
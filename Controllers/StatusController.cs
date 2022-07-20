using exemploDB2.Data;
using exemploDB2.Models;
using exemploDB2.Services;
using Microsoft.AspNetCore.Mvc;

namespace exemploDB2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ExemploDB2Context context;

        public StatusController(ExemploDB2Context context)
        {
            this.context = context;
        }

        [HttpPost()]
        public ActionResult<StatusPedidoResponse> StatusDoPedido(StatusPedidoDto statusDto)
        {
            var service = new StatusPedidoService(context);
            var response = service.MudancaDeStatus(statusDto);
            return Ok(response);
        }
    }
}
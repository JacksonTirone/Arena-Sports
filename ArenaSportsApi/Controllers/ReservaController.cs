using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservaController : BaseController
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _reservaService = reservaService;
        }

        [HttpPost("RealizarReserva")]
        public async Task<IActionResult> RealizarReserva(RealizarReservaVMRequest request)
        {
            var response = await _reservaService.RealizarReserva(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("CancelarReserva")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var response = await _reservaService.CancelarReserva(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
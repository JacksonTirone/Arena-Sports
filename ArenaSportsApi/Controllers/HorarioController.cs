using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HorarioController : BaseController
    {
        private readonly IHorarioService _horarioService;

        public HorarioController(IHorarioService horarioService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _horarioService = horarioService;
        }

        [HttpPost("ConfigurarHorarioQuadra")]
        public async Task<IActionResult> ConfigurarHorarioQuadra(ConfigHorarioQuadraVMRequest request)
        {
            var response = await _horarioService.ConfigHorarioQuadra(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetHorariosQuadras")]
        public async Task<IActionResult> GetHorariosQuadras(HorariosQuadrasVMRequest request)
        {
            var response = await _horarioService.GetHorariosQuadras(request);
            if (!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetConfiguracaoHorarios")]
        public async Task<IActionResult> GetConfiguracaoHorarios(ConfiguracaoHorarioVMRequest request)
        {
            var response = await _horarioService.GetConfigHorario(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteConfiguracaoHorario")]
        public async Task<IActionResult> DeleteConfiguracaoHorario(int id)
        {
            var response = await _horarioService.DeleteConfigHorario(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
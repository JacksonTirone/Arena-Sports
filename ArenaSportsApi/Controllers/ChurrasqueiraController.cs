using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChurrasqueiraController : BaseController
    {
        private readonly IChurrasqueiraService _churrasService;

        public ChurrasqueiraController(IChurrasqueiraService churrasService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _churrasService = churrasService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _churrasService.GetAll();
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetAllDisponiveis")]
        public async Task<IActionResult> GetAllDisponiveis(ChurrasqueirasDisponiveisVMRequest request)
        {
            var response = await _churrasService.GetAllDisponiveis(request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("CadastrarAtualizarChurrasqueira")]
        public async Task<IActionResult> CadastrarAtualizarChurrasqueira(ChurrasqueiraVMRequest request)
        {
            var response = await _churrasService.CadastrarAtualizarChurrasqueira(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteChurrasqueira")]
        public async Task<IActionResult> DeleteChurrasqueira(int id)
        {
            var response = await _churrasService.DeleteChurrasqueira(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllPacote")]
        public async Task<IActionResult> GetAllPacote()
        {
            var response = await _churrasService.GetAllPacote();
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("CadastrarAtualizarPacoteChurrasqueira")]
        public async Task<IActionResult> CadastrarAtualizarPacoteChurrasqueira(ChurrasqueiraPacoteVMRequest request)
        {
            var response = await _churrasService.CadastrarAtualizarPacote(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeletePacoteChurrasqueira")]
        public async Task<IActionResult> DeletePacoteChurrasqueira(int id)
        {
            var response = await _churrasService.DeletePacoteChurrasqueira(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuadraController : BaseController
    {
        private readonly IQuadraService _quadraService;

        public QuadraController(IQuadraService quadraService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _quadraService = quadraService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _quadraService.GetAll();
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllOpcionais")]
        public async Task<IActionResult> GetAllOpcionais()
        {
            var response = await _quadraService.GetAllOpcionais();
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("CadastrarAtualizarQuadra")]
        public async Task<IActionResult> CadastrarAtualizarQuadra(CadastrarQuadraVMRequest request)
        {
            var response = await _quadraService.CadastrarAtualizarQuadra(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("CadastrarAtualizarOpcionalQuadra")]
        public async Task<IActionResult> CadastrarAtualizarOpcionalQuadra(QuadraItemOpcionalVMRequest request)
        {
            var response = await _quadraService.CadastrarAtualizarItemQuadra(usuarioLogado, request);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteQuadra")]
        public async Task<IActionResult> DeleteQuadra(int id)
        {
            var response = await _quadraService.DeleteQuadra(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteOpcionalQuadra")]
        public async Task<IActionResult> DeleteOpcionalQuadra(int id)
        {
            var response = await _quadraService.DeleteOpcionalQuadra(usuarioLogado, id);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
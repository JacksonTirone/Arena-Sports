using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _userService;

        public UsuarioController(IUsuarioService userService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _userService = userService;
        }

        [HttpPost("CadastrarClienteSimples")]
        public async Task<IActionResult> CadastrarClienteSimples(CadastroSimplesVMRequest request)
        {
            var response = await _userService.CadastroSimples(UsuarioAdapter.ViewModelToModel(request));
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetTop10Clientes")]
        public async Task<IActionResult> GetTop10Clientes(string cpfNome)
        {
            var response = await _userService.GetTop10Clientes(cpfNome);
            return Ok(response);
        }
    }
}
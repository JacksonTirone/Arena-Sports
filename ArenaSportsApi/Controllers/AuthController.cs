using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArenaSportsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _userService;
        public AuthController(IUsuarioService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UsuarioRegisterVMRequest request)
        {
            var response = await _userService.Register(UsuarioAdapter.ViewModelToModel(request), request.Password);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ConfirmarUsuario")]
        public async Task<IActionResult> ConfirmarUsuario(HashVMRequest hash)
        {
            var response = await _userService.ConfirmarUsuario(hash.Hash);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioLoginVMRequest request)
        {
            var response = await _userService.Login(request.Email, request.Password);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("EsqueciSenha")]
        public async Task<IActionResult> EsqueciSenha(string email)
        {
            var response = await _userService.EsqueciSenha(email);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("EsqueciEmail")]
        public async Task<IActionResult> EsqueciEmail(string cpf)
        {
            var response = await _userService.EsqueciEmail(cpf);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArenaSportsApi.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        public UsuarioLogado usuarioLogado;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                usuarioLogado = JsonConvert.DeserializeObject<UsuarioLogado>(claims[2].Value);
            }
        }
    }
}
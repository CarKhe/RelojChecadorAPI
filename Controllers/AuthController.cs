using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;   
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO usuario)
        {
            if (usuario is null)
                return BadRequest();

            var user = await _service.Login(usuario);
            if (user is null)
                return Unauthorized("Credenciales inválidas");

            string token = _service.GenerarToken(user);

            return Ok(new
            {
                token,
                user = new {
                    user.idUser,
                    user.nombre,
                    user.idRol
                }
            });
            
        }
    }
}

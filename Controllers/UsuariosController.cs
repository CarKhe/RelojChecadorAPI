using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _service;
        private readonly IMensajesDB _mensajeDB;
        private static string MODELO = "USUARIO";
        public UsuariosController(IUsuariosService service, IMensajesDB mensajeDB)
        {
            _service = service;
            _mensajeDB = mensajeDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosTablaDTOs>>> GetUsers()
        {
            var usuarios = await _service.GetUsuarios();
            if (!usuarios.Any()) return NotFound();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UsuariosModificarDTO>>> GetOneUser(long id)
        {
            var usuario =  await _service.GetOneUsuario(id);
            if(!usuario.Any()) return NotFound(new { Mensaje = _mensajeDB.MensajeNoEncontrado(MODELO) });
            return Ok(usuario.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] UsuariosCrearDTOs usuario)
        {
            var(result, errores) = await _service.PostUsuario(usuario);
            if (!result)
            {
                return BadRequest(new
                {
                    Mensaje = "Error en las llaves foráneas.",
                    Errores = errores
                });
            }
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO, 0) });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> ToogleUser(long id)
        {
            var result = await _service.ToogleUser(id);
            if (!result)
                return NotFound(new { Mensaje = _mensajeDB.MensajeNoEncontrado(MODELO) });

            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO, 1) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipo(long id, [FromBody] UsuariosCrearDTOs usuarios)
        {
            var (isSuccess, errores) = await _service.UpdateUser(id, usuarios);
            if (!isSuccess)
                return BadRequest(new { Mensaje = "Error al actualizar el equipo.", Errores = errores });
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO,3) });

        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAreaController : ControllerBase
    {
        private readonly IUsuarioAreaService _service;
        private readonly IMensajesDB _mensajeDB;
        private readonly string MODELO = "USUARIOAREA";
        public UsuarioAreaController(IUsuarioAreaService service, IMensajesDB mensajesDB)
        {
            _service = service;
            _mensajeDB = mensajesDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioAreaTablaDTOs>>> GetUsuarioArea()
        {
            var usuarios = await _service.GetUsuarioAreas();
            if (!usuarios.Any()) return BadRequest();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuarioArea([FromBody] UsuarioAreaCrearDto usuarioArea)
        {
            var (result, errores) = await _service.PostUsuarioArea(usuarioArea);
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
        public async Task<IActionResult> ToogleUsuarioArea(long id)
        {
            var result = await _service.ToogleUsuarioArea(id);
            if (!result)
                return NotFound(new { Mensaje = _mensajeDB.MensajeNoEncontrado(MODELO) });
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO, 1) });
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(long id, [FromBody] UsuarioAreaCrearDto usuarioArea)
        {
            var (isSuccess, errores) = await _service.UpdateUsuarioArea(id, usuarioArea);
            if (!isSuccess)
                return BadRequest(new { Mensaje = "Error al actualizar el equipo.", Errores = errores });
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO,3) });

        }
    }
}

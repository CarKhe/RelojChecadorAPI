using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _service;
        private readonly IMensajesDB _mensajeDB;
        private readonly string MODELO = "ASISTENCIA";
        public AsistenciaController(IAsistenciaService service, IMensajesDB mensajesDB)
        {
            _service = service;
            _mensajeDB = mensajesDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsistenciaTablaDTOs>>> GetAsistencia()
        {
            var equipos = await _service.GetAsistencia();
            return Ok(equipos);
        }

        [HttpGet("last/{cant}")]
        public async Task<ActionResult<IEnumerable<AsistenciaToDashboard>>> GetLastAsistencias(int cant)
        {
            var equipos = await _service.GetLastAsistencias(cant);
            return Ok(equipos);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsistencia([FromBody] AsistenciaCrearDto asistencia)
        {
            bool result;
            List<string> errores;
            (result, errores) = await _service.PostAsistencia(asistencia);
            if (!result)
            {
                return BadRequest(new
                {
                    Mensaje = "Error en las llaves foráneas.",
                    Errores = errores
                });
            }
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO,0) });
        }

        [HttpPost("lastStatus")]
        public async Task<IActionResult> GetLastStatus([FromBody]LastRegisterDTO lastRegister)
        {
            LastRegisterReturnDTO status = await _service.GetLastAsistenciaStatus(lastRegister);
            if(status.movimiento == 99) status.movimiento = 1;
            return Ok(status);
        }
    }
}

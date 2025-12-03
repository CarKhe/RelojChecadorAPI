using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantillaHorarioController : ControllerBase
    {
        private readonly IPlantillaHorarioService _service;
        private readonly IMensajesDB _mensajeDB;
        private static string MODELO = "PLANTILLA HORARIO";
        public PlantillaHorarioController(IPlantillaHorarioService service, IMensajesDB mensajesDB)
        {
            _service = service;
            _mensajeDB = mensajesDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioPlantillaTablaDto>>> GetHorariosPlantilla()
        {
            var plantillas = await _service.GetPlantillas();
            if (!plantillas.Any()) return NotFound();
            return Ok(plantillas);
        }

        [HttpPost]
        public async Task<IActionResult> PostHorarioPlantilla([FromBody] HorarioPlantillaCrearDto horarioPlantilla)
        {
            var(result, errores) = await _service.PostHorarioPlantilla(horarioPlantilla);
            if(!result){
                return BadRequest(new
                {
                    Mensaje = "Error en las llaves foráneas.",
                    Errores = errores
                });
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ToogleHorarioPlantilla(long id)
        {
            var result = await _service.ToogleHorarioPlantilla(id);
            if (!result)
                return NotFound(new { Mensaje = _mensajeDB.MensajeNoEncontrado(MODELO) });

            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO, 1) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorarioPlantilla(long id, [FromBody] HorarioPlantillaCrearDto horarioPlantilla)
        {
            var (isSuccess, errores) = await _service.UpdateHorarioPlantilla(id, horarioPlantilla);
            if (!isSuccess)
                return BadRequest(new { Mensaje = "Error al actualizar el equipo.", Errores = errores });
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO,3) });
        }

    }
}

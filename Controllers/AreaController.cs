using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _service;
        private readonly IMensajesDB _mensajeDB;
        private static string MODELO = "AREA";
        public AreaController(IAreaService service, IMensajesDB mensajesDB)
        {
            _service = service;
            _mensajeDB = mensajesDB;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreasTablaDTOs>>> GetArea()
        {
            var area = await _service.GetAreas();
            if (!area.Any()) return BadRequest();
            return Ok(area);
        }

        [HttpGet("chip")]
        public async Task<ActionResult<AreaChipDTOs[]>> GetAreaChip()
        {
            var areaChip = await _service.GetAreaChip();
            if(!areaChip.Any()) return BadRequest();
            return Ok(areaChip);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] AreasCrearDTOs area)
        {
            var (result, errores) = await _service.PostArea(area);
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
        public async Task<IActionResult> ToogleArea(long id)
        {
            var result = await _service.ToogleArea(id);
            if (!result)
                return NotFound(new { Mensaje = _mensajeDB.MensajeNoEncontrado(MODELO) });

            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO, 1) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(long id, [FromBody] AreasCrearDTOs area)
        {
            var (isSuccess, errores) = await _service.UpdateArea(id, area);
            if (!isSuccess)
                return BadRequest(new { Mensaje = "Error al actualizar el equipo.", Errores = errores });
            return Ok(new { Mensaje = _mensajeDB.MensajeModificarDB(MODELO,3) });

        }
        
        
    }
}

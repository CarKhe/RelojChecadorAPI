using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        private readonly IDownloadsService _service;
        public DownloadsController(IDownloadsService service)
        {
            _service = service;
        }

        [HttpPost("RegistroHoras")]
        public async Task<IActionResult> DescargarAsistencias([FromBody] AsistenciasFechasDTO rango)
        {
             var archivoExcel = await _service.DescargarAsistencias(rango);
            return File(
                archivoExcel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Usuarios.xlsx"
            );
        }
    }
}

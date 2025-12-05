using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IDownloadsService
{
    Task<byte[]> DescargarAsistencias([FromBody] AsistenciasFechasDTO rango);
}

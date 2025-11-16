using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IAsistenciaService
{
    Task<IEnumerable<AsistenciaTablaDTOs>> GetAsistencia();
    Task<(bool isSuccess, List<string> errores)> PostAsistencia([FromBody] AsistenciaCrearDto asistenciaCrear);
}

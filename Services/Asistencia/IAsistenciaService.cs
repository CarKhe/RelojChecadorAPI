using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IAsistenciaService
{
    Task<IEnumerable<AsistenciaTablaDTOs>> GetAsistencia();

    Task<IEnumerable<AsistenciaToDashboard>> GetLastAsistencias(int cant);

    Task<IEnumerable<AsistenciaToDashboard>> GetLastAsistenciasByUser(int userId, int cant);

    Task<LastRegisterReturnDTO> GetLastAsistenciaStatus(LastRegisterDTO lastRegister);

    Task<(bool isSuccess, List<string> errores)> PostAsistencia([FromBody] AsistenciaCrearDto asistenciaCrear);

}

using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IAreaService
{
    Task<IEnumerable<AreasTablaDTOs>> GetAreas();
    Task<(bool isSuccess, List<string> errores)> PostArea([FromBody] AreasCrearDTOs area);
    Task<bool> ToogleArea(long id);
    Task<(bool isSuccess, List<string> errores)> UpdateArea(long id, AreasCrearDTOs area);
}

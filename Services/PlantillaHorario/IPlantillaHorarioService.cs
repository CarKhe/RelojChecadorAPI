using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IPlantillaHorarioService
{
    Task<IEnumerable<HorarioPlantillaTablaDto>> GetPlantillas();
    Task<(bool isSuccess, List<string> errores)> PostHorarioPlantilla([FromBody] HorarioPlantillaCrearDto horarioPlantilla);
    Task<bool> ToogleHorarioPlantilla(long id);
    Task<(bool isSuccess, List<string> errores)>UpdateHorarioPlantilla(long id, [FromBody] HorarioPlantillaCrearDto horarioPlantilla);
}

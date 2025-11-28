using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IUsuarioAreaService
{
    Task<IEnumerable<UsuarioAreaTablaDTOs>> GetUsuarioAreas();
    (bool isSuccess, List<string> errores) PostUsuarioArea([FromBody] UsuarioAreaCrearDto UsuarioArea);

    Task<bool> ToogleUsuarioArea(long id);

}

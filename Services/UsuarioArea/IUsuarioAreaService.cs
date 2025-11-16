using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IUsuarioAreaService
{
    Task<IEnumerable<UsuarioAreaTablaDTOs>> GetUsuarioAreas();
    Task<(bool isSuccess, List<string> errores)> PostUsuarioArea([FromBody] UsuarioAreaCrearDto UsuarioArea);

    Task<bool> ToogleUsuarioArea(long id);
    Task<(bool isSuccess, List<string> errores)> UpdateUsuarioArea(long id, UsuarioAreaCrearDto usuarioArea);

}

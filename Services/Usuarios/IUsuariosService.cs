using Microsoft.AspNetCore.Mvc;

namespace relojChecadorAPI;

public interface IUsuariosService
{
    Task<IEnumerable<UsuariosTablaDTOs>> GetUsuarios();
    Task<IEnumerable<UsuariosModificarDTO>>GetOneUsuario(long id);
    Task<(bool isSuccess, List<string> errores)> PostUsuario([FromBody] UsuariosCrearDTOs usuario);
    Task<bool> ToogleUser(long id);
    Task<bool> DeleteUUID(long id);
    Task<(bool isSuccess, List<string> errores)> UpdateUser(long id, UsuariosCrearDTOs usuario);

}

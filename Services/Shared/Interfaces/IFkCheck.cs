namespace relojChecadorAPI;

public interface IFkCheck
{
    (bool isValid, List<string>) FkUsuario(UsuariosCrearDTOs usuario);
    (bool isValid, List<string>) FkUsuarioArea(UsuarioAreaCrearDto usuarioArea);
    (bool isValid, List<string>) FkAsistenica(AsistenciaCrearDto asistencia);
    (bool isValid, List<string>) FkUsuarioAreaAsistencia(AsistenciaCrearDto asistencia);


    
}

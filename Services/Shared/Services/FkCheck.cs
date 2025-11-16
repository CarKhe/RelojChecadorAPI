using System.Runtime.CompilerServices;
using relojChecadorAPI.Data;

namespace relojChecadorAPI;

public class FkCheck : IFkCheck
{
    private readonly DbRelojChecadorContext _context;
    private readonly IMensajesDB _mensajeDB;
    public FkCheck(DbRelojChecadorContext context, IMensajesDB mensajeDB)
    {
        _context = context;
        _mensajeDB = mensajeDB;
    }
    public (bool isValid, List<string>) FkUsuario(UsuariosCrearDTOs usuario)
    {
        var errores = new List<string>();
        if (!RolExiste(usuario.idRol))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("ROL"));
        return (!errores.Any(), errores);
    }

    public (bool isValid, List<string>) FkUsuarioArea(UsuarioAreaCrearDto usuarioArea)
    {
        var errores = new List<string>();
        if (!AreaExiste(usuarioArea.idArea))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("AREA"));
        if (!UsuarioExiste(usuarioArea.idUsuario))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("USUARIO"));
        return (!errores.Any(), errores);

    }

    public (bool isValid, List<string>) FkAsistenica(AsistenciaCrearDto asistencia)
    {
        bool isValid;
        var errores = new List<string>();
        if (!AreaExiste(asistencia.idArea))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("AREA"));
        if (!UsuarioExiste(asistencia.idUsuario))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("USUARIO"));
        if (!MovimientoExiste(asistencia.idMovimiento))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("MOVIMIENTO"));
        isValid = errores.Any() ? false : true;
        return (isValid, errores);
    }

    public (bool isValid, List<string>) FkUsuarioAreaAsistencia(AsistenciaCrearDto asistencia)
    {
        var errores = new List<string>();
        var (isValid,errores1) = FkAsistenica(asistencia);
        if(!isValid)
            return (isValid, errores);
        if (!AreaExiste(asistencia.idArea))
                errores.Add(_mensajeDB.LlaveForaneaNoExiste("AREA"));
        if (!UsuarioExiste(asistencia.idUsuario))
            errores.Add(_mensajeDB.LlaveForaneaNoExiste("USUARIO"));
        errores.AddRange(errores1);
        isValid = errores.Any() ? false : true;
        return (isValid, errores);
    }

    private bool RolExiste(long id)
    {
        return _context.TblRoles.Any(r => r.IdRol == id);
    }
    private bool UsuarioExiste(long id)
    {
        return _context.TblUsuarios.Any(u => u.IdUsuario == id);
    }
    private bool AreaExiste(long id)
    {
        return _context.TblAreas.Any(a => a.IdArea == id);
    }
    private bool MovimientoExiste(long id)
    {
        return _context.TblTipoMovimientos.Any(m => m.IdMovimiento == id);
    }


}

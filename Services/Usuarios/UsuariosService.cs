using System.Net.Sockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class UsuariosService : IUsuariosService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IMapper _mapper;
    private readonly ISyntaxisDB _syntaxisDB;
    private readonly IFkCheck _fkCheck;
    private readonly IMensajesDB _mensajeDB;
    private static string MODELO = "USUARIO"; 
    public UsuariosService(DbRelojChecadorContext context, IMapper mapper,
                        ISyntaxisDB syntaxisDB, IFkCheck fkCheck, IMensajesDB mensajesDB)
    {
        _context = context;
        _mapper = mapper;
        _syntaxisDB = syntaxisDB;
        _fkCheck = fkCheck;
        _mensajeDB = mensajesDB;
    }

    public async Task<IEnumerable<UsuariosTablaDTOs>> GetUsuarios()
    {
        var query = GetUsuariosQuery();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<UsuariosCrearDTOs>> GetOneUsuario(long id)
    {
        var query = GetUserByID(id);
        return await query.ToListAsync();
    }

    public async Task<(bool isSuccess, List<string> errores)> PostUsuario([FromBody] UsuariosCrearDTOs usuario)
    {
        bool isValidFk;
        List<string> errores;
        var usuarioMap = _mapper.Map<TblUsuario>(usuario);
        usuarioMap.Nombre = _syntaxisDB.StringUpper(usuarioMap.Nombre);
        (isValidFk, errores) = _fkCheck.FkUsuario(usuario);
        if (!isValidFk)
        {
            return (isValidFk,errores);
        }
        _context.Add(usuarioMap);
        await _context.SaveChangesAsync();
        return (isValidFk,[]);


    }

    public async Task<bool> ToogleUser(long id)
    {
        var usuario = await _context.TblUsuarios.FindAsync(id);
        if (usuario == null) return false;
        usuario.Activo = usuario.Activo == 1UL ? 0UL : 1UL;
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<(bool isSuccess, List<string> errores)> UpdateUser(long id, UsuariosCrearDTOs usuario)
    {
        bool isValidFk;
        List<string> errores;
        var usr = await _context.TblUsuarios.FindAsync(id);
        if (usr == null)
            return (false, new List<string> { _mensajeDB.MensajeNoEncontrado(MODELO) });
        (isValidFk, errores) = _fkCheck.FkUsuario(usuario);
        if (!isValidFk)
            return (isValidFk, errores);
        _mapper.Map(usuario, usr);
        usr.Nombre = _syntaxisDB.StringUpper(usr.Nombre);
        await _context.SaveChangesAsync();
        return (isValidFk, errores);
    }

    private IQueryable<UsuariosTablaDTOs> GetUsuariosQuery()
    {
        return from u in _context.TblUsuarios
               join r in _context.TblRoles on u.IdRol equals r.IdRol
               select new UsuariosTablaDTOs
               {
                    id = u.IdUsuario,
                    nombre = u.Nombre,
                    correo = u.Correo,
                    telefono = u.Telefono,
                    rol = r.RolName,
                    fechaCreacion = u.FechaCreacion,
                    activo = u.Activo
               };
    }

    private IQueryable<UsuariosCrearDTOs> GetUserByID(long id)
    {
        return from u in _context.TblUsuarios
                where u.IdUsuario == id
                select new UsuariosCrearDTOs
                {
                    nombre = u.Nombre,
                    passwordHash = u.PasswordHash,
                    idRol = u.IdRol,
                    telefono = u.Telefono
                };
    }


}

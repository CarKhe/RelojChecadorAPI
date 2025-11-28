using System.Linq;
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
    private readonly IUsuarioAreaService _usuarioAreaService;
    private static string MODELO = "USUARIO"; 
    public UsuariosService(DbRelojChecadorContext context, IMapper mapper,
            ISyntaxisDB syntaxisDB, IFkCheck fkCheck, IMensajesDB mensajesDB,
            IUsuarioAreaService usuarioAreaService)
    {
        _context = context;
        _mapper = mapper;
        _syntaxisDB = syntaxisDB;
        _fkCheck = fkCheck;
        _mensajeDB = mensajesDB;
        _usuarioAreaService = usuarioAreaService;
    }

    public async Task<IEnumerable<UsuariosTablaDTOs>> GetUsuarios()
    {
        var query = GetUsuariosQuery();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<UsuariosModificarDTO>> GetOneUsuario(long id)
    {
        var query = GetUserByID(id);
        return await query.ToListAsync();
    }

    public async Task<(bool isSuccess, List<string> errores)> PostUsuario([FromBody] UsuariosCrearDTOs usuario)
    {
        //Variables
        bool isValidFk;
        List<string> errores;

        //Validaciones
        (isValidFk, errores) = _fkCheck.FkUsuario(usuario);
        if (!isValidFk)
            return (isValidFk,errores);
        
        //Metodo de Transaccion
        using var trx = await _context.Database.BeginTransactionAsync();

        try
        {
            //Guardar Usuario
            var usuarioMap = _mapper.Map<TblUsuario>(usuario);
            usuarioMap.Nombre = _syntaxisDB.StringUpper(usuarioMap.Nombre);
            _context.Add(usuarioMap);
            await _context.SaveChangesAsync();

            //Guardar Areas Asigandas al usuario
            foreach (var idArea in usuario.idAreas)
            {
                var usuarioArea = new UsuarioAreaCrearDto
                {
                    idUsuario = usuarioMap.IdUsuario,
                    idArea = idArea
                };
                (isValidFk, errores) = _usuarioAreaService.PostUsuarioArea(usuarioArea);
                if(!isValidFk)
                {
                    await trx.RollbackAsync();
                    return (false,errores);
                }
                var usuarioAreaMap = _mapper.Map<TblUsuarioArea>(usuarioArea);
                _context.Add(usuarioAreaMap);
            }
            await _context.SaveChangesAsync();
            await trx.CommitAsync();
            return (isValidFk,[]);
        }
        catch (Exception ex)
        {
            await trx.RollbackAsync();
            return (false, new List<string> { ex.Message });
        }
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
        //Variables
        bool isValidFk;
        List<string> errores;
        //Validaciones
        (isValidFk, errores) = _fkCheck.FkUsuario(usuario);
        if (!isValidFk)
            return (isValidFk, errores);  
        var usr = await _context.TblUsuarios.FindAsync(id);
        if (usr == null)
            return (false, new List<string> { _mensajeDB.MensajeNoEncontrado(MODELO) });
        _mapper.Map(usuario, usr);
        usr.Nombre = _syntaxisDB.StringUpper(usr.Nombre);
        await _context.SaveChangesAsync();

        //Revision de las Areas

        var areasNuevas = usuario.idAreas                           // Lo que viene del frontend
            .Select(x => (long)x)
            .ToList();

        var areasBD = await _context.TblUsuarioAreas                // Lo que ya existe en BD
            .Where(x => x.IdUsuario == id)
            .ToListAsync();
    
        //Crear/Activar áreas
        foreach (var idArea in areasNuevas)
        {
            var existente = areasBD.FirstOrDefault(x => x.IdArea == idArea);

            if (existente == null)
            {
                // No existe → agregar
                _context.TblUsuarioAreas.Add(new TblUsuarioArea
                {
                    IdUsuario = id,
                    IdArea = idArea
                });
            }
            else
            {
                // Ya existe → activar si está inactivo
                if (existente.Activo == 0)
                {
                    existente.Activo = 1;
                }
            }
        }

        //Desactivar áreas que ya no vienen seleccionadas
        foreach (var area in areasBD)
        {
            if (!areasNuevas.Contains(area.IdArea) && area.Activo == 1)
            {
                area.Activo = 0;
            }
        }

        // Guardar cambios
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

    private IQueryable<UsuariosModificarDTO> GetUserByID(long id)
    {
        return from u in _context.TblUsuarios
                where u.IdUsuario == id
                select new UsuariosModificarDTO
                {
                    id = Convert.ToInt32(u.IdUsuario),
                    nombre = u.Nombre,
                    passwordHash = u.PasswordHash,
                    idRol = Convert.ToInt32(u.IdRol),
                    telefono = u.Telefono,
                    idAreas = _context.TblUsuarioAreas
                            .Where(ua => ua.IdUsuario == id)
                            .Where(ua => ua.Activo == 1)
                            .Select(ua => (int)ua.IdArea)
                            .ToList()
                };
    }




}

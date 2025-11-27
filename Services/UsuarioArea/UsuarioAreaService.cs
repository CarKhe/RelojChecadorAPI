
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class UsuarioAreaService : IUsuarioAreaService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IMapper _mapper;
    private readonly IFkCheck _fkCheck;
    private readonly IMensajesDB _mensajeDb;
    private readonly string MODELO = "USUARIOAREA";
    public UsuarioAreaService(DbRelojChecadorContext context, IMapper mapper,
        IFkCheck fkCheck, IMensajesDB mensajeDB)
    {
        _context = context;
        _mapper = mapper;
        _fkCheck = fkCheck;
        _mensajeDb = mensajeDB;
    }
    public async Task<IEnumerable<UsuarioAreaTablaDTOs>> GetUsuarioAreas()
    {
        var areas = GetUsuarioAreaQuery();
        return await areas.ToListAsync();
    }

    public  (bool isSuccess, List<string> errores)PostUsuarioArea([FromBody] UsuarioAreaCrearDto UsuarioAreaDto)
    {
        bool isValidFk;
        List<string> errores;
        (isValidFk, errores) = _fkCheck.FkUsuarioArea(UsuarioAreaDto);
        if (!isValidFk)
        {
            return (isValidFk, errores);
        }
        return (isValidFk,errores);
    }

    public async Task<bool> ToogleUsuarioArea(long id)
    {
        var usuarioArea = await _context.TblUsuarioAreas.FindAsync(id);
        if (usuarioArea == null) return false; 
        usuarioArea.Activo = usuarioArea.Activo == 1UL ? 0UL : 1UL; 
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool isSuccess, List<string> errores)> UpdateUsuarioArea(long id, UsuarioAreaCrearDto usuarioArea)
    {
        bool isValidFk;
        List<string> errores;
        var usr = await _context.TblUsuarioAreas.FindAsync(id);
        if (usr == null)
            return (false, new List<string> { _mensajeDb.MensajeNoEncontrado(MODELO) });
        (isValidFk, errores) = _fkCheck.FkUsuarioArea(usuarioArea);
        if (!isValidFk)
            return (isValidFk, errores);
        _mapper.Map(usuarioArea, usr);
        await _context.SaveChangesAsync();
        return (isValidFk, errores);
    }

    private IQueryable<UsuarioAreaTablaDTOs> GetUsuarioAreaQuery()
    {
        return  from ua in _context.TblUsuarioAreas
                join u in _context.TblUsuarios on ua.IdUsuario equals u.IdUsuario
                join a in _context.TblAreas on ua.IdArea equals a.IdArea
                where ua.Activo == 1
                select new UsuarioAreaTablaDTOs
                {
                   usuario = u.Nombre,
                   area = a.Nombre,
                   fechaCreacion = ua.FechaCreacion,
                   activo = ua.Activo
                };
    }
}

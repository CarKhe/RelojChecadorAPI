
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class AreaService : IAreaService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IMapper _mapper;
    private readonly ISyntaxisDB _syntaxisDB;
    private readonly IFkCheck _fkCheck;
    private readonly IMensajesDB _mensajeDB;
    private string MODELO = "AREA";
    public AreaService(DbRelojChecadorContext context, IMapper mapper,
                        ISyntaxisDB syntaxisDB, IFkCheck fkCheck, IMensajesDB mensajesDB)
    {
        _context = context;
        _mapper = mapper;
        _syntaxisDB = syntaxisDB;
        _fkCheck = fkCheck;
        _mensajeDB = mensajesDB;
    }

    public async Task<IEnumerable<AreasTablaDTOs>> GetAreas()
    {
        var areas = GetAreaQuery();
        return await areas.ToListAsync();

    }

    public async Task<IEnumerable<AreaChipDTOs>> GetAreaChip()
    {
        var areaChip = GetAreaChipQuery();
        return await areaChip.ToListAsync();
    }

    public async Task<(bool isSuccess, List<string> errores)> PostArea([FromBody] AreasCrearDTOs area)
    {
        bool isValidFk = true;
        List<string> errores = [];
        var areaMap = _mapper.Map<TblArea>(area);
        areaMap.Nombre = _syntaxisDB.StringUpper(areaMap.Nombre);
        _context.Add(areaMap);
        await _context.SaveChangesAsync();
        return (isValidFk,errores);
    }

    public async Task<bool> ToogleArea(long id)
    {
        var usuario = await _context.TblAreas.FindAsync(id);
        if (usuario == null) return false;
        usuario.Activo = usuario.Activo == 1UL ? 0UL : 1UL;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool isSuccess, List<string> errores)> UpdateArea(long id, AreasCrearDTOs area)
    {
        bool isValidFk = true;
        List<string> errores = [];
        var areaMap = await _context.TblAreas.FindAsync(id);
        if (areaMap == null)
            return (false, new List<string> { _mensajeDB.MensajeNoEncontrado(MODELO) });
        _mapper.Map(area, areaMap);
        areaMap.Nombre = _syntaxisDB.StringUpper(areaMap.Nombre);
        await _context.SaveChangesAsync();
        return (isValidFk, errores);
    }

    private IQueryable<AreasTablaDTOs> GetAreaQuery()
    {
        return from a in _context.TblAreas
                select new AreasTablaDTOs
                {
                    id = Convert.ToInt32(a.IdArea),
                    nombre = a.Nombre,
                    descripcion = a.Descripcion,
                    centroLat = a.CentroLat,
                    centroLon = a.CentroLon,
                    radio = a.Radio,
                    fechaCreacion = a.FechaCreacion,
                    activo = a.Activo
                };
    }

    private IQueryable<AreaChipDTOs> GetAreaChipQuery()
    {
        return from a in _context.TblAreas
            where a.Activo == 1
            select new AreaChipDTOs
            {
              id = Convert.ToInt32(a.IdArea),
              label = a.Nombre  
            };
    }
}


using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class PlantillaHorarioService : IPlantillaHorarioService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IMapper _mapper;
    private readonly IFkCheck _fkCheck;
    private readonly ISyntaxisDB _syntaxisDB;
    private readonly IMensajesDB _mensajeDB;
    private static string MODELO = "HORARIO PLANTILLA";
    public PlantillaHorarioService(DbRelojChecadorContext context, IMapper mapper,
            IFkCheck fkCheck, ISyntaxisDB syntaxisDB, IMensajesDB mensajesDB)
    {
        _context = context;
        _mapper = mapper;
        _fkCheck = fkCheck;
        _syntaxisDB = syntaxisDB;
        _mensajeDB = mensajesDB;
        
    }
    public async Task<IEnumerable<HorarioPlantillaTablaDto>> GetPlantillas()
    {
        var query = GetPlantillasQuery();
        return await query.ToListAsync();
    }

    public async Task<(bool isSuccess, List<string> errores)> PostHorarioPlantilla([FromBody] HorarioPlantillaCrearDto horarioPlantilla)
    {
        bool isValidFk;
        List<string> errores;


        using var trx = await _context.Database.BeginTransactionAsync();
        try
        {
            var horarioPlantillaMap = _mapper.Map<TblHorarioPlantilla>(horarioPlantilla);
            horarioPlantillaMap.Nombre = _syntaxisDB.StringUpper(horarioPlantillaMap.Nombre);
            _context.Add(horarioPlantillaMap);
            await _context.SaveChangesAsync();
            //Revisar llaves foraneas de los detalles
            foreach (DetalleHorarioPlantillaCrearDto detalle in horarioPlantilla.detalleHorarioPlantillaCrear)
            {
                detalle.idHorarioPlantilla = horarioPlantillaMap.IdHorarioPlantilla;
                (isValidFk, errores) = _fkCheck.FkPlantillaHorario(detalle);
                if (!isValidFk){
                    await trx.RollbackAsync();
                    return (false,errores);          
                }
                var detalleHorarioPlantillaMap = _mapper.Map<TblDetalleHorarioPlantilla>(detalle);
                _context.Add(detalleHorarioPlantillaMap);
            }
            await _context.SaveChangesAsync();
            await trx.CommitAsync();
            return (true,[]);
        }
        catch(Exception ex)
        {
            await trx.RollbackAsync();
            return (false, new List<string> { ex.Message});
        }
    }

    public async Task<bool> ToogleHorarioPlantilla(long id)
    {
        var plantilla = await _context.TblHorarioPlantillas.FindAsync(id);
        if (plantilla == null) return false;
        plantilla.Activo = plantilla.Activo == 1UL ? 0UL : 1UL;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool isSuccess, List<string> errores)> UpdateHorarioPlantilla(long id, [FromBody] HorarioPlantillaCrearDto horarioPlantilla)
    {
        bool isValidFk;
        List<string> errores;
        using var trx = await _context.Database.BeginTransactionAsync();
        try
        {
            var plantilla = await _context.TblHorarioPlantillas.FindAsync(id);
            if (plantilla == null)
                return (false, new List<string> { _mensajeDB.MensajeNoEncontrado(MODELO) });
            var horarioPlantillaMap = _mapper.Map<TblHorarioPlantilla>(horarioPlantilla);
            plantilla.Nombre = _syntaxisDB.StringUpper(plantilla.Nombre);
            await _context.SaveChangesAsync();

            foreach (DetalleHorarioPlantillaCrearDto detalle in horarioPlantilla.detalleHorarioPlantillaCrear)
            {
                if(detalle.idHorarioPlantilla > 0)
                {
                    // UPDATE
                    var detalleExistente = plantilla.TblDetalleHorarioPlantillas
                        .FirstOrDefault(d => d.IdDetalleHorarioPlantilla == detalle.idHorarioPlantilla);

                    if (detalleExistente == null)
                    {
                        await trx.RollbackAsync();
                        return (false, new List<string> { "El detalle no existe pero se envió un ID." });
                    }

                    _mapper.Map(detalle, detalleExistente);
                }
                else
                {
                    detalle.idHorarioPlantilla = id;
                    (isValidFk, errores) = _fkCheck.FkPlantillaHorario(detalle);
                    if (!isValidFk){
                        await trx.RollbackAsync();
                        return (false,errores);          
                    }
                    var detalleHorarioPlantillaMap = _mapper.Map<TblDetalleHorarioPlantilla>(detalle);
                    _context.Add(detalleHorarioPlantillaMap);
                }
            }

            await trx.CommitAsync();
            return (true,[]);
        }
        catch(Exception ex)
        {
            await trx.RollbackAsync();
            return (false, new List<string> { ex.Message});
        }
    }

    private IQueryable<HorarioPlantillaTablaDto> GetPlantillasQuery()
    {
        return
            from hp in _context.TblHorarioPlantillas
            join dhp in _context.TblDetalleHorarioPlantillas
                on hp.IdHorarioPlantilla equals dhp.IdHorarioPlantilla into detalles
            select new HorarioPlantillaTablaDto
            {
                idHorarioPlantilla = (int)hp.IdHorarioPlantilla,
                nombre = hp.Nombre,
                activo = hp.Activo,
                detalleHorarioPlantillaTabla =
                    detalles
                        .Where(d => d.Activo == 1)
                        .Join(
                            _context.TblTipoMovimientos,
                            d => d.IdMovimiento,
                            m => m.IdMovimiento,
                            (d, m) => new DetalleHorarioPlantillaTablaDto
                            {
                                idHorarioPlantilla = d.IdHorarioPlantilla,
                                movimiento = m.Movimiento,
                                diaSemana = d.DiaSemana,
                                hora = d.Hora,
                                margenAntes = d.MargenAntes,
                                margenDespues = d.MargenDespues,
                                laboral = d.Laboral,
                                activo = d.Activo
                            }
                        )
                        .ToList()
            };
    }


}

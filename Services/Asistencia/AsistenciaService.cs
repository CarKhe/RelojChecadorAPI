
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class AsistenciaService : IAsistenciaService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IFkCheck _fkCheck;
    private readonly IMapper _mapper;
    private readonly IMensajesDB _mensajeDb;
    //private readonly string MODELO = "USUARIOAREA";
    public AsistenciaService(DbRelojChecadorContext context, IFkCheck fkCheck,
                                IMensajesDB mensajesDB, IMapper mapper)
    {
        _context = context;
        _fkCheck = fkCheck;
        _mensajeDb = mensajesDB;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AsistenciaTablaDTOs>> GetAsistencia()
    {
        var query = GetAsistenciaQuery();
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<AsistenciaToDashboard>> GetLastAsistencias(int cant)
    {
        var query = GetLastAsistenciaQuery(cant);
        return await query.ToListAsync();
    }

    public async Task<int> GetLastAsistenciaStatus(LastRegisterDTO lastRegister)
    {
        var lastStatus = await GetLastAsistenciaStatus(lastRegister.idUsuario);
        if (lastStatus == null) return 99;
        return lastStatus.Value;
    }

    public async Task<(bool isSuccess, List<string> errores)> PostAsistencia([FromBody] AsistenciaCrearDto asistenciaCrear)
    {
        //Obtener las Areas Asignadas del Cliente
        List<int> areasAsignadas = AreasAsignadas(asistenciaCrear.idUsuario);

        foreach (int areaRevision in areasAsignadas)
        {
            asistenciaCrear.idArea = areaRevision;

            //Verificar si el usuario y Area existen
            var (isValidFk, errores) = _fkCheck.FkUsuarioAreaAsistencia(asistenciaCrear);
            if (!isValidFk)
                continue;

            //verificar si en la tabla de tbl_UsuarioArea existe la relacion
            bool relacion = IssetUsuarioArea(asistenciaCrear);
            if (!relacion)
                continue;

            //verificar si la latitud y longitud seleccionadas estan dentro del rango del area
            ulong dentroZona = DentroZona(asistenciaCrear);
            if(dentroZona != 1UL)
                continue;

            var valorGuardar = _mapper.Map<TblAsistencium>(asistenciaCrear);
            valorGuardar.DentroZona = dentroZona;

            _context.Add(valorGuardar);
            await _context.SaveChangesAsync();
            return (isValidFk, errores);
        }
        return (false, ["Fuera de Rango de cualquier Area"]);
    }


    private ulong DentroZona(AsistenciaCrearDto asistenciaCrear)
    {
        var areaSelect = GetCenLatLonRad(asistenciaCrear).FirstOrDefault();
        if (areaSelect == null) return 0UL;
        
        const double R = 6371000; //radio del mundo
        double dLat = (Convert.ToDouble(asistenciaCrear.latitud) - Convert.ToDouble(areaSelect.latitudCentral)) 
                    * Math.PI / 180;
        double dLon = (Convert.ToDouble(asistenciaCrear.longitud) - Convert.ToDouble(areaSelect.longitudCentral))
                    * Math.PI / 180;
        // Fórmula de Haversine
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
               Math.Cos(Convert.ToDouble(areaSelect.latitudCentral) * Math.PI / 180.0)
               * Math.Cos(Convert.ToDouble(asistenciaCrear.latitud) * Math.PI / 180.0) *
               Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Distancia entre el punto y el centro (en metros)
        double distancia = R * c;

        // Devuelve true si está dentro o justo en el límite
        return distancia <= Convert.ToDouble(areaSelect.radio) ? 1UL: 0UL;
    }

    private IQueryable<AsistenciaTablaDTOs> GetAsistenciaQuery()
    {
        return from a in _context.TblAsistencia
               join u in _context.TblUsuarios on a.IdUsuario equals u.IdUsuario
               join ar in _context.TblAreas on a.IdArea equals ar.IdArea
               join tp in _context.TblTipoMovimientos on a.IdMovimiento equals tp.IdMovimiento
               select new AsistenciaTablaDTOs
               {
                   usuario = u.Nombre,
                   area = ar.Nombre,
                   movimiento = tp.Movimiento,
                   latitud = a.Latitud,
                   longitud = a.Longitud,
                   distanciaCentro = a.DistanciaCentro,
                   dentroZona = a.DentroZona,
                   fechaHora = a.FechaHora
               };
    }

    private IQueryable<AsistenciaToDashboard> GetLastAsistenciaQuery(int cant)
    {
        return (from a in _context.TblAsistencia
               join u in _context.TblUsuarios on a.IdUsuario equals u.IdUsuario
               join ar in _context.TblAreas on a.IdArea equals ar.IdArea
               join tp in _context.TblTipoMovimientos on a.IdMovimiento equals tp.IdMovimiento
               orderby a.FechaHora descending
               select new AsistenciaToDashboard
               {
                   usuario = u.Nombre,
                   area = ar.Nombre,
                   movimiento = tp.Movimiento,
                   dentroZona = a.DentroZona,
                   fechaHora = a.FechaHora
               })
               .Take(cant);
    }



    private IQueryable<AreaDto>GetCenLatLonRad(AsistenciaCrearDto asistencia)
    {
        return from a in _context.TblAreas
                where a.IdArea == asistencia.idArea
                select new AreaDto
                {
                    latitudCentral = a.CentroLat,
                    longitudCentral = a.CentroLon,
                    radio = a.Radio
                };
    }

    private bool IssetUsuarioArea(AsistenciaCrearDto asistencia)
    {
        return ( from a in _context.TblUsuarioAreas
            where a.IdArea == asistencia.idArea
            && a.IdUsuario == asistencia.idUsuario
            && a.Activo == 1
            select a).Any();
    }

    private List<int> AreasAsignadas(long id)
    {
        var query = from ua in _context.TblUsuarioAreas
            where ua.IdUsuario == id
            && ua.Activo == 1
            select (int)ua.IdArea;

        return query.ToList();
    }

    private async Task<int?> GetLastAsistenciaStatus(int idUser)
    {
        return await _context.TblAsistencia
            .Where(a => a.IdUsuario == idUser && a.DentroZona == 1)
            .OrderByDescending(a => a.IdAsistencia)
            .Select(a => (int?)a.IdMovimiento)
            .FirstOrDefaultAsync();
    }



    


}

using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using relojChecadorAPI.Data;

namespace relojChecadorAPI;

public class DownloadsService : IDownloadsService
{

    private readonly DbRelojChecadorContext _context;
    public DownloadsService(DbRelojChecadorContext context)
    {
        _context = context;
    }
    public async Task<byte[]> DescargarAsistencias([FromBody] AsistenciasFechasDTO rango)
    {
        return await Task.Run(() =>
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Horario");
            
            var asistenciaExcels = GetAsistenciasRangoList(rango);

            foreach (var asistencia in asistenciaExcels)
            {
                ws.Cell(1, 1).Value = "ID";
                ws.Cell(1, 2).Value = "Nombre";
                

                ws.Cell(2, 1).Value = 1;
                ws.Cell(2, 2).Value = "CarKhe";
                ws.Cell(2, 3).Value = DateTime.Now;
            }

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Nombre";
            

            ws.Cell(2, 1).Value = 1;
            ws.Cell(2, 2).Value = "CarKhe";
            ws.Cell(2, 3).Value = DateTime.Now;

            using var stream = new MemoryStream();
            wb.SaveAs(stream);

            return stream.ToArray();
        });
    }

    private IQueryable<AsistenciaExcelDTO> GetAsistenciasRangoList(AsistenciasFechasDTO rango)
    {
        return from u in _context.TblUsuarios
            join a in _context.TblAsistencia
                    on u.IdUsuario equals a.IdUsuario into detalle
            select new AsistenciaExcelDTO
            {
                idUsuario = u.IdUsuario,
                nombreEmpleado = u.Nombre,
                detalleAsistencias = 
                (from d in detalle
                    where d.FechaHora >= rango.fechaInicio &&
                        d.FechaHora <= rango.fechaFin

                    join ar in _context.TblAreas
                        on d.IdArea equals ar.IdArea

                    join m in _context.TblTipoMovimientos
                        on d.IdMovimiento equals m.IdMovimiento

                    select new DetalleAsistenciaExcelDTO
                    {
                        area = ar.Nombre,          
                        movimiento = m.Movimiento,     
                        fechaHora = d.FechaHora
                    }).ToList()
            };
    }



}

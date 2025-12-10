using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //Variables
            var usuariosList = GetUsuariosListed().ToList();
            int numFila = 1;

            //TITULO
            ws.Column("A").Width = 30;
            ws.Column("B").Width = 30;
            ws.Column("C").Width = 30;
            ws.Range("A"+numFila+":C"+numFila).Merge();
            ws.Cell("A"+numFila).Value = "REGISTRO ASISTENCIA: "
            + rango.fechaInicio.ToString("dd/MM/yyyy") +" - "+ rango.fechaFin.ToString("dd/MM/yyyy");
            ws.Cell("A"+numFila).Style.Font.Bold = true;
            numFila ++;

            foreach (var usuario in usuariosList)
            {
                int horasTotalesSuma = 0;
                TimeSpan TotalHorasTrabajadasSemana = TimeSpan.Zero;
                ws.Cell("A"+numFila).Value = "ID EMPLEADO:" + usuario.idUsuario;
                ws.Cell("A"+numFila).Style.Font.Bold = true;
                ws.Range("B"+numFila+":C"+numFila).Merge();
                ws.Cell("B"+numFila).Value = "NOMBRE EMPLEADO: " + usuario.nombreEmpleado;
                ws.Range("B"+numFila).Style.Font.Bold = true;
                numFila ++;
                horasTotalesSuma = numFila;
                ws.Cell("A"+numFila).Value = "HORAS SEMANALES TRABAJADAS: ";
                numFila ++;

                var busqueda = new AssistenciaExcelSearchDateDTO
                {
                    idUsuario = usuario.idUsuario,
                    fechas = rango
                };

                var asistenciasTotales = GetAsistenciasRango(busqueda).ToList();

                var groupByDia = asistenciasTotales
                    .GroupBy(a => a.fechaHora.Date)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.ToList());

                DateTime fecha = rango.fechaInicio.Date;

                while (fecha <= rango.fechaFin.Date)
                {
                    ws.Range("A" + numFila + ":C" + numFila).Merge();
                    ws.Cell("A" + numFila).Value = "DIA: " + fecha.ToString("dddd dd-MM-yyyy");
                    ws.Cell("A"+numFila).Style.Font.Bold = true;
                    ws.Cell("A"+ numFila).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Cell("A"+ numFila).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    numFila++;
                    if (groupByDia.TryGetValue(fecha, out var movimientosDia))
                    {
                        var tiempos = CalcularHorasDia(movimientosDia);
                        TotalHorasTrabajadasSemana += tiempos.HorasTrabajadas;
                        var totalTiempo = tiempos.HorasDescanso + tiempos.HorasTrabajadas;
                        ws.Range("A"+ numFila+":C"+numFila).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws.Cell("A" + numFila).Value = "TIEMPO TRABAJADO: " + tiempos.HorasTrabajadas;
                        ws.Cell("B" + numFila).Value = "TIEMPO DESCANSADO: " + tiempos.HorasDescanso;
                        ws.Cell("C" + numFila).Value = "TIEMPO TOTAL: "+ totalTiempo;
                        numFila++;
                        ws.Cell("A"+numFila).Value = "AREA";
                        ws.Cell("B"+numFila).Value = "MOVIMIENTO";
                        ws.Cell("C"+numFila).Value = "HORA";
                        numFila ++;
                        foreach (var mov in movimientosDia)
                        {
                            ws.Cell("A" + numFila).Value = mov.area;
                            ws.Cell("B" + numFila).Value = mov.movimiento;
                            ws.Cell("C" + numFila).Value = mov.fechaHora.ToString("hh:mm tt");
                            numFila++;
                        }
                    }
                    else
                    {
                        numFila++;
                        var rango = ws.Range("A" + numFila + ":C" + numFila);
                        rango.Merge();                       
                        rango.Value = "SIN REGISTROS";       

                        rango.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rango.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        rango.Style.Font.SetBold();        
                        numFila++;
                    }

                    numFila++; 
                    ws.Cell("B"+horasTotalesSuma).Value = TotalHorasTrabajadasSemana;
                    fecha = fecha.AddDays(1);
                }

                numFila++;
            }

            using var stream = new MemoryStream();
            wb.SaveAs(stream);

            return stream.ToArray();
        });
    }


    private ResultadoHorasDia CalcularHorasDia(List<DetalleAsistenciaExcelDTO> movimientosDelDia)
    {
        // Ordenar por si acaso no vienen ordenados
        var movimientos = movimientosDelDia
            .OrderBy(m => m.fechaHora)
            .ToList();

        TimeSpan horasTrabajadas = TimeSpan.Zero;
        TimeSpan horasDescanso = TimeSpan.Zero;

        // Recorrer pares consecutivos
        for (int i = 0; i < movimientos.Count - 1; i++)
        {
            var actual = movimientos[i];
            var siguiente = movimientos[i + 1];

            // Entrada → Salida = trabajo
            if (actual.movimiento.ToUpper() == "ENTRADA" &&
                siguiente.movimiento.ToUpper() == "SALIDA")
            {
                horasTrabajadas += (siguiente.fechaHora - actual.fechaHora);
            }
            // Salida → Entrada = descanso
            else if (actual.movimiento.ToUpper() == "SALIDA" &&
                    siguiente.movimiento.ToUpper() == "ENTRADA")
            {
                horasDescanso += (siguiente.fechaHora - actual.fechaHora);
            }
        }

        return new ResultadoHorasDia
        {
            HorasTrabajadas = horasTrabajadas,
            HorasDescanso = horasDescanso
        };
    }

    private IQueryable<AsistenciaExcelUSuarioDTO> GetUsuariosListed()
    {
        return from u in _context.TblUsuarios
            where u.Activo == 1
            select new AsistenciaExcelUSuarioDTO
            {
                idUsuario = u.IdUsuario,
                nombreEmpleado = u.Nombre
            };
    }

    private IQueryable<DetalleAsistenciaExcelDTO> GetAsistenciasRango(AssistenciaExcelSearchDateDTO busqueda)
    {

        return from a in _context.TblAsistencia
            join ar in _context.TblAreas on a.IdArea equals ar.IdArea
            join m in _context.TblTipoMovimientos on a.IdMovimiento equals m.IdMovimiento
            join u in _context.TblUsuarios on a.IdUsuario equals u.IdUsuario
            where a.IdUsuario == busqueda.idUsuario
                && a.FechaHora >= busqueda.fechas.fechaInicio
                && a.FechaHora < busqueda.fechas.fechaFin
                && u.Activo == 1
            select new DetalleAsistenciaExcelDTO
            {
                area = ar.Nombre,
                movimiento = m.Movimiento,
                fechaHora = a.FechaHora
            };
    }





}

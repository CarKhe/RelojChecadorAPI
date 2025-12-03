using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// tabla para crear plantilla de horarios para asistencias de usuarios
/// </summary>
public partial class TblHorarioPlantilla
{
    public long IdHorarioPlantilla { get; set; }

    public string Nombre { get; set; } = null!;

    public ulong Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<TblDetalleHorarioPlantilla> TblDetalleHorarioPlantillas { get; set; } = new List<TblDetalleHorarioPlantilla>();

    public virtual ICollection<TblEmpleadoHorario> TblEmpleadoHorarios { get; set; } = new List<TblEmpleadoHorario>();
}

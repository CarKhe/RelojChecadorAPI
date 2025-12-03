using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// Registro que empleado esta asignado a que plantilla asignadoa
/// </summary>
public partial class TblEmpleadoHorario
{
    public long IdEmpleadoHorario { get; set; }

    public long IdUsuario { get; set; }

    public long IdHorarioPlantilla { get; set; }

    public DateTime FechaCreacion { get; set; }

    public ulong Activo { get; set; }

    public virtual TblHorarioPlantilla IdHorarioPlantillaNavigation { get; set; } = null!;

    public virtual TblUsuario IdUsuarioNavigation { get; set; } = null!;
}

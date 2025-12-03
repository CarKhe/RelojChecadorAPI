using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// Detalle de la plantilla de horarios
/// </summary>
public partial class TblDetalleHorarioPlantilla
{
    public long IdDetalleHorarioPlantilla { get; set; }

    public long IdHorarioPlantilla { get; set; }

    public long IdMovimiento { get; set; }

    public int DiaSemana { get; set; }

    public TimeOnly Hora { get; set; }

    public int MargenAntes { get; set; }

    public int MargenDespues { get; set; }

    public ulong Laboral { get; set; }

    public ulong Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual TblHorarioPlantilla IdHorarioPlantillaNavigation { get; set; } = null!;

    public virtual TblTipoMovimiento IdMovimientoNavigation { get; set; } = null!;
}

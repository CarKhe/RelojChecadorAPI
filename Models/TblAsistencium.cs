using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// tabla con el control de los registros  de asistencia
/// </summary>
public partial class TblAsistencium
{
    public long IdAsistencia { get; set; }

    public long IdUsuario { get; set; }

    public long IdArea { get; set; }

    public long IdMovimiento { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public decimal DistanciaCentro { get; set; }

    public ulong DentroZona { get; set; }

    public DateTime FechaHora { get; set; }

    public virtual TblArea IdAreaNavigation { get; set; } = null!;

    public virtual TblTipoMovimiento IdMovimientoNavigation { get; set; } = null!;

    public virtual TblUsuario IdUsuarioNavigation { get; set; } = null!;
}

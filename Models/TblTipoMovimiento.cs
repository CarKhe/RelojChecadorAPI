using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// catalogo de los movimientos disponibles
/// </summary>
public partial class TblTipoMovimiento
{
    public long IdMovimiento { get; set; }

    public string Movimiento { get; set; } = null!;

    public virtual ICollection<TblAsistencium> TblAsistencia { get; set; } = new List<TblAsistencium>();

    public virtual ICollection<TblDetalleHorarioPlantilla> TblDetalleHorarioPlantillas { get; set; } = new List<TblDetalleHorarioPlantilla>();
}

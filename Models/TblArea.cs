using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// definicion de las Areas donde se pueden registrar el chequeo
/// </summary>
public partial class TblArea
{
    public long IdArea { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal CentroLat { get; set; }

    public decimal CentroLon { get; set; }

    public int Radio { get; set; }

    public DateTime FechaCreacion { get; set; }

    public ulong Activo { get; set; }

    public virtual ICollection<TblAsistencium> TblAsistencia { get; set; } = new List<TblAsistencium>();

    public virtual ICollection<TblUsuarioArea> TblUsuarioAreas { get; set; } = new List<TblUsuarioArea>();
}

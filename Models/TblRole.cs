using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// Tabla encargada de asignar los roles que tendra cada usuario
/// </summary>
public partial class TblRole
{
    public long IdRol { get; set; }

    public string RolName { get; set; } = null!;

    public virtual ICollection<TblUsuario> TblUsuarios { get; set; } = new List<TblUsuario>();
}

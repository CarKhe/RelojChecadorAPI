using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// asigna el area que se desee que se registre el usuario
/// </summary>
public partial class TblUsuarioArea
{
    public long IdUsuarioArea { get; set; }

    public long IdUsuario { get; set; }

    public long IdArea { get; set; }

    public DateTime FechaCreacion { get; set; }

    public ulong Activo { get; set; }

    public virtual TblArea IdAreaNavigation { get; set; } = null!;

    public virtual TblUsuario IdUsuarioNavigation { get; set; } = null!;
}

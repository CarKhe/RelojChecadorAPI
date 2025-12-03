using System;
using System.Collections.Generic;

namespace relojChecadorAPI.Models;

/// <summary>
/// Registro de los Usuarios asignados
/// </summary>
public partial class TblUsuario
{
    public long IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public long IdRol { get; set; }

    public string Telefono { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public ulong Activo { get; set; }

    public string? DeviceUuid { get; set; }

    public virtual TblRole IdRolNavigation { get; set; } = null!;

    public virtual ICollection<TblAsistencium> TblAsistencia { get; set; } = new List<TblAsistencium>();

    public virtual ICollection<TblEmpleadoHorario> TblEmpleadoHorarios { get; set; } = new List<TblEmpleadoHorario>();

    public virtual ICollection<TblUsuarioArea> TblUsuarioAreas { get; set; } = new List<TblUsuarioArea>();
}

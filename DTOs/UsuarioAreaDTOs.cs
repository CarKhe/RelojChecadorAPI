namespace relojChecadorAPI;

public class UsuarioAreaTablaDTOs
{
    public string usuario { get; set; } = string.Empty;
    public string area { get; set; } = string.Empty;
    public DateTime fechaCreacion { get; set; }
    public ulong activo { get; set; }
}

public class UsuarioAreaCrearDto
{
    public long idUsuario { get; set; }
    public long idArea{ get; set; }
}

namespace relojChecadorAPI;

public class UsuariosCrearDTOs
{
    public string nombre { get; set; } = string.Empty;
    public string correo { get; set; } = "example@gmail.com";
    public string passwordHash { get; set; } = string.Empty;
    public long idRol { get; set; }
    public string telefono { get; set; } = string.Empty;
}

public class UsuariosTablaDTOs
{
    public long id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string correo { get; set; } = string.Empty;
    public string telefono { get; set; } = string.Empty;
    public string rol { get; set; } = string.Empty;
    public DateTime fechaCreacion { get; set; } 
    public ulong activo { get; set; }
}

namespace relojChecadorAPI;

public class UsuariosCrearDTOs
{
    public string nombre { get; set; } = string.Empty;
    public string correo { get; set; } = "example@gmail.com";
    public string passwordHash { get; set; } = string.Empty;
    public long idRol { get; set; }
    public string telefono { get; set; } = string.Empty;
    public List<int> idAreas {get; set;} = new();
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

public class UsuariosModificarDTO
{
    public int id { get; set;}
    public string nombre { get; set; } = string.Empty;

    public string passwordHash { get; set; } = string.Empty;

    public int idRol { get; set; }

    public string telefono { get; set; } = string.Empty;

    
}

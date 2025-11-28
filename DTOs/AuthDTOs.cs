namespace relojChecadorAPI;

public class LoginDTO
{
    public string telefono { get; set; } = string.Empty;
    public string passwordHash { get; set; } = string.Empty;
}

public class UserAuthLoginDTO
{
    public int idUser { get; set; }
    public string nombre { get; set; } = string.Empty;
    public int idRol { get; set; }
}

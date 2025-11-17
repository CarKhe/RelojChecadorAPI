using Microsoft.AspNetCore.Identity;

namespace relojChecadorAPI;

public class RolesDto
{
    public long idRol { get; set; }
    public string rol { get; set; } = string.Empty;
}

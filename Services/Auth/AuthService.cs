
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using relojChecadorAPI.Data;

namespace relojChecadorAPI;

public class AuthService : IAuthService
{
    private readonly DbRelojChecadorContext _context;
    private readonly IConfiguration _config;
    private readonly IHashingService _hash;
    public AuthService(DbRelojChecadorContext context, IConfiguration config,
                        IHashingService hash)
    {
        _context = context;
        _config = config;
        _hash = hash;
    }
    public async Task<UserAuthLoginDTO?> Login(LoginDTO usuario)
    {
        var user = await _context.TblUsuarios
            .FirstOrDefaultAsync(u => u.Telefono == usuario.telefono 
             && u.Activo == 1);
            
        if (user == null) return null; 

        bool passwordCorrecta = _hash.Verify(usuario.passwordHash, user.PasswordHash);
        if(!passwordCorrecta) return null;

        if(user.DeviceUuid == null)
        {
            user.DeviceUuid = usuario.deviceUUID;
            await _context.SaveChangesAsync();
        }
        else
        {
            if (user.DeviceUuid != usuario.deviceUUID) return null;
        }

        return await LoginQuery(usuario).FirstOrDefaultAsync();
    }


    public string GenerarToken(UserAuthLoginDTO user)
    {
        return CreateJWT(user);
    }

    private string CreateJWT(UserAuthLoginDTO user)
    {
        var roleName = GetRoleNameByID(user.idRol);

        if (string.IsNullOrEmpty(roleName))
        {
            throw new Exception("El rol especificado no existe.");
        }
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new Exception("JWT Key not found")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.idUser.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.nombre ?? string.Empty),
            new Claim(ClaimTypes.Role, roleName) 
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private IQueryable<UserAuthLoginDTO> LoginQuery(LoginDTO usuario)
    {
        return from u in _context.TblUsuarios
            join r in _context.TblRoles on u.IdRol equals r.IdRol
            where u.Telefono == usuario.telefono
            select new UserAuthLoginDTO
            {
                idUser = (int)u.IdUsuario,
                nombre = u.Nombre,
                idRol = (int)u.IdRol,
                rol = r.RolName.ToLower()
            };
    }

    private string? GetRoleNameByID(int idRol)
    {
        return _context.TblRoles
            .Where(r => r.IdRol == idRol)
            .Select(r => r.RolName)      
            .FirstOrDefault();
    }
}

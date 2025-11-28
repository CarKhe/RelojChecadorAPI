namespace relojChecadorAPI;

public interface IAuthService
{
    Task<UserAuthLoginDTO?> Login(LoginDTO usuario);

    string GenerarToken(UserAuthLoginDTO user);
}

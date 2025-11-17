namespace relojChecadorAPI;

public interface IRolesService
{
    Task<IEnumerable<RolesDto>> GetRoles();
}

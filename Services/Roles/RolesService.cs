
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using relojChecadorAPI.Data;

namespace relojChecadorAPI;

public class RolesService : IRolesService
{
    private readonly DbRelojChecadorContext _context;
   private readonly IMapper _mapper;
    private readonly ISyntaxisDB _syntaxisDB;
    //private static string MODELO = "ROL"; 

    public RolesService(DbRelojChecadorContext context, IMapper mapper,
                        ISyntaxisDB syntaxisDB)
    {
        _context = context;
        _mapper = mapper;
        _syntaxisDB = syntaxisDB;

    }

    public async Task<IEnumerable<RolesDto>> GetRoles()
    {
        var query = GetRolesQuery();
        return await query.ToListAsync();
    }

    private IQueryable<RolesDto> GetRolesQuery()
    {
        return from r in _context.TblRoles
                where r.IdRol != 1
                select new RolesDto
                {
                    idRol = r.IdRol,
                    rol = r.RolName
                };
    }
}

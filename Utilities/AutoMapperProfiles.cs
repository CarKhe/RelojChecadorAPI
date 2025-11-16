using System.Runtime.CompilerServices;
using AutoMapper;
using relojChecadorAPI.Models;

namespace relojChecadorAPI;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        MapperUsuarios();
        MapperArea();
        MapperUsuarioArea();
        MapperAsistencia();
    }

    private void MapperUsuarios()
    {
        CreateMap<UsuariosCrearDTOs, TblUsuario>();
    }
    private void MapperArea()
    {
        CreateMap<AreasCrearDTOs, TblArea>();
    }
    private void MapperUsuarioArea()
    {
        CreateMap<UsuarioAreaCrearDto, TblUsuarioArea>();
    }
    private void MapperAsistencia()
    {
        CreateMap<AsistenciaCrearDto,TblAsistencium>();
    }
}

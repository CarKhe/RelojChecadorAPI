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
        MapperHorarioPlantilla();
        MapperDetalleHorarioPlantilla();
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

    private void MapperHorarioPlantilla()
    {
        CreateMap<HorarioPlantillaCrearDto,TblHorarioPlantilla>();
    }

    private void MapperDetalleHorarioPlantilla()
    {
        CreateMap<DetalleHorarioPlantillaCrearDto,TblDetalleHorarioPlantilla>();
    }
}

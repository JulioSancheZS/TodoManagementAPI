using AutoMapper;
using TodoManagementAPI.DTO;
using TodoManagementAPI.Models;

namespace TodoManagementAPI.Utilidades
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino => destino.NombreCompleto,
                opt => opt.MapFrom(origen => origen.PrimerNombre + " " + origen.PrimerNombre));

            CreateMap<EstadoTarea, EstadoTareaDTO>().ReverseMap();

            CreateMap<Tarea, TareaDTO>()
                .ForMember(destino => destino.EstadoTarea, opt => opt.MapFrom(origen => origen.IdEstadoTareaNavigation.Description)
                );

            CreateMap<TareaDTO, Tarea>()
                .ForMember(destino => destino.IdEstadoTareaNavigation, opt => opt.Ignore()
                );
        }
    }
}

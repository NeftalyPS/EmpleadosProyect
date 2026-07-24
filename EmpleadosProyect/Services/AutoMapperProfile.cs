using AutoMapper;
using EmpleadosApp.Models.DTOs;
using EmpleadosApp.Models.Entities;

namespace EmpleadosApp.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Empleado, EmpleadoDto>()
                .ForMember(d => d.DepartamentoNombre, opt => opt.MapFrom(s => s.Departamento != null ? s.Departamento.Nombre : null));

            CreateMap<EmpleadoCreateDto, Empleado>();
            CreateMap<EmpleadoUpdateDto, Empleado>();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
        }
    }
}
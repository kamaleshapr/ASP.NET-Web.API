using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Business.DTO;
using TaskManagement.Domain.Models;

namespace TaskManagement.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
            CreateMap<Employee, EmployeeEditDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => src.Role.ToString())
            );

            CreateMap<TaskItem, TaskItemCreateDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemUpdateDto>().ReverseMap();

            // To ignore this concatenation in AutoMapper, define a FullName property in Employee with getter only.
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(
                des => des.Status,
                opt => opt.MapFrom(src => src.Status.ToString())
                )
                .ForMember(
                des => des.AssignedToEmployeeName,
                opt => opt.MapFrom(src =>
                src.AssignedTo != null ? $"{src.AssignedTo.FirstName} {src.AssignedTo.LastName}" : string.Empty));
        }
    }
}

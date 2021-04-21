using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CourseDto>().ForMember(dest => dest.EndDate,
                  from => from.MapFrom(c => c.StartDate.AddMonths(3))).ReverseMap();
            CreateMap<Module, ModuleDto>().ForMember(dest => dest.EndDate,
                  from => from.MapFrom(c => c.StartDate.AddMonths(1))).ReverseMap();

            CreateMap<CourseForCreationDto,Course>();
            CreateMap<ModuleForCreationDto, Module>();


        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Aplication.Dtos.Authorization;
using Tickets.Infraestructure.Identity.Models;

namespace Tickets.Infraestructure.Identity.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>()
                .ForMember(x => x.UserName, dest => dest.MapFrom(y => y.Email))
                .ReverseMap();
        }
    }
}

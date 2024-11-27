using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Mapping
{
    public class MappingProfile :Profile
    {
        MappingProfile()
        {
            CreateMap<UserDto, User>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}

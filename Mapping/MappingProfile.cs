using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserDto>()
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
             .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "*******")); // Ignore Password when mapping User -> UserDto
           

            //CreateMap<UserDto, User>(); // Map UserDto back to User
            CreateMap<UserDto, User>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));// Map PasswordHash (assuming you have a PasswordHash field) 

        }
    }
}

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


            // Map CategoryDto to Category
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

            // Map Category to CategoryDto
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

            // Mapping for ProductDto (assuming you have a Product model)
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        }
    }
}

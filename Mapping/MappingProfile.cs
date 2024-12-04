using AutoMapper;
using DarazApp.DTOs;
using DarazApp.Models;

namespace DarazApp.Mapping
{
    public class MappingProfile : Profile
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


            // Mapping Order to OrderDto

            CreateMap<Order, OrderDto>()
          .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
          .ForMember(dest => dest.NumOfItems, opt => opt.MapFrom(src => src.NumOfItems))
          .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
          .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
          .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount)); // Map calculated TotalAmount
         

            // Mapping OrderDto to Order (without TotalAmount as it's calculated in service)
            CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Ignore TotalAmount
            .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())  // Ignore OrderStatus
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Default to active
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // Set ModifiedAt to current time
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId));

            // Mapping from Order to OrderOutputDto
            CreateMap<Order, OrderOutputDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))  // Assuming 'Id' is the primary key in Order
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.NumOfItems, opt => opt.MapFrom(src => src.NumOfItems))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}

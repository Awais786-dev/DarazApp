using DarazApp.DbContext;
using DarazApp.Mapping;
using DarazApp.Models;
using DarazApp.Repositories.UserRepositories;
using DarazApp.Service.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register services here
//builder.Services.AddScoped<IUserRepository, UserRepository>(); // Shared instance for user data
//builder.Services.AddScoped<IUserService, UserService>(); // Register the service // New instance for each request


builder.Services.AddAutoMapper(typeof(MappingProfile)); // Registers your profile

builder.Services.AddDbContext<UserDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add Identity services
builder.Services.AddIdentity<User, IdentityRole>()  // Use your custom User class
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders(); // Optional, adds token providers (e.g., for email confirmation, password reset, etc.)



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using DarazApp.Models;
namespace DarazApp.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

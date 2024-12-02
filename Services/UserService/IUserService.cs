using DarazApp.Models;

namespace DarazApp.Services.UserService
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user);
        Task<string> GenerateConfirmationLink(string email);
    }
}

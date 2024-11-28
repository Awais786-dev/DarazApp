using DarazApp.Models;

namespace DarazApp.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<string> GenerateConfirmationLink(string email, string token = null);

    }
}

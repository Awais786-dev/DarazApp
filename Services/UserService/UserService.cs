using DarazApp.Models;
using DarazApp.Repositories.UserRepositories;

namespace DarazApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterUser(User user)
        {
            // Check if the user with the same email already exists
            User existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this mail already exist.");
            }

            User newUser = new()
            {
                FullName = user.FullName,
                Address = user.Address,
                UserName = user.Email,
                Email = user.Email,
            };

            // Otherwise, proceed with adding the user
            return await _userRepository.AddUser(newUser);
        }

        public async Task<string> GenerateConfirmationLink(string email)
        {
            return await _userRepository.GenerateConfirmationLink(email);
        }
    }
}

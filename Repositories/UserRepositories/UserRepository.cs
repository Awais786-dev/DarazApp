using DarazApp.DbContext;
using DarazApp.Models;
using Microsoft.AspNetCore.Identity;

namespace DarazApp.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;
       // private List<User> users;



        public UserRepository(UserDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> AddUser(User user)
        {
            IdentityResult result = await _userManager.CreateAsync(user); //For email confirmation
            if (!result.Succeeded)
            {
                // Combine all errors into one exception or return them as an error response
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Retrieve the saved user by their email (or another identifier like username if needed)
            User savedUser = await _userManager.FindByEmailAsync(user.Email);
            return savedUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateConfirmationLink(string email, string token = null)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (token == null)
            {
                token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            }

            // Generate a token for email confirmation
         //   token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Create the confirmation URL (this would typically be a URL to a front-end page)
            var confirmationLink = $"http://localhost:7230/api/Auth/confirm-email?token={token}&email={email}";

            return confirmationLink;
        }
    }
}

using DarazApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DarazApp.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        // Constructor that accepts configuration to get the issuer and secret key
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Define the claims that will be included in the JWT token
            var claims = new[]
            {
                new Claim("fullName", user.FullName.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Standard (.NET)
                new Claim(ClaimTypes.Name, user.Email),
            };

            // Get the secret key from the configuration (make sure it matches what you defined in appsettings.json)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Create signing credentials with the symmetric key and algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token with the required information
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Issuer URL (http://localhost:7230/)
                audience: _configuration["Jwt:Audience"], // Audience URL (http://localhost:7230/)
                claims: claims, // Add the claims to the token
                expires: DateTime.Now.AddHours(1), // Set expiration time (1 hour in this case)
                signingCredentials: creds // Use signing credentials to sign the token
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

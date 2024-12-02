using DarazApp.Responses;
using DarazApp.Models; // Assuming your `User` model is here
using DarazApp.Services; // Assuming the UserService is here
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DarazApp.DTOs;
using DarazApp.Services.UserService;
using DarazApp.Helpers;
using DarazApp.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using DarazApp.Services.TokenService;

namespace DarazApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;




        public AuthController(IUserService userService, IMapper mapper, IEmailService emailService, UserManager<User> userManager, ITokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
            _tokenService= tokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Authenticate the user using email and password
                User user = await _userManager.FindByEmailAsync(loginDto.Email);

                // If authentication fails, return Unauthorized
                if (user == null)
                {
                    return BadRequest<string>(AuthResponseMessages.InvalidCredientials);
                }

                bool passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!passwordValid)
                {
                    return BadRequest<string>(AuthResponseMessages.InvalidCredientials);
                }


                // Generate JWT token for the authenticated user
                string token = _tokenService.GenerateToken(user);
                var response = new { Token = token };

                return Ok(response, AuthResponseMessages.UserLoginSuccess);
            }
            catch(Exception ex)
            {
                return BadRequest<string>(AuthResponseMessages.InvalidCredientials, new List<string> { ex.Message });  

            }
        }


        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {

                User user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

                if (user == null)
                {
                    return BadRequest<ApiResponse<string>>("User not found.");
                }
 
                string confirmationLink = await _userService.GenerateConfirmationLink(user.Email);
                await _emailService.SendConfirmationEmail(user.Email, confirmationLink);

                return Ok<string>(AuthResponseMessages.PasswordReset);

            }
            catch (Exception ex)
            {
                return BadRequest<ApiResponse<string>>($"An error occurred: {ex.Message}");
            }
        }




        // POST: api/auth/signup
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp([FromBody] UserDto userDto)
        {
            ActionResult validationResponse = ValidateModel(userDto);
            if (validationResponse != null) return validationResponse;

            try
            {
                // Map UserDto to User entity
                User user = _mapper.Map<User>(userDto);

                // Call the service to register the user
                User createdUser = await _userService.RegisterUser(user);

                if (createdUser == null)
                {
                    return BadRequest<UserDto>(UserResponseMessages.ErrorOccurred);
                }

               string confirmationLink = await _userService.GenerateConfirmationLink(user.Email);

                // implementation of email sending logic
                await _emailService.SendConfirmationEmail(user.Email, confirmationLink);


                UserDto savedUserDto = _mapper.Map<UserDto>(createdUser);  // Map the saved User entity back to UserDto

                // Return success response with the created user data
                return Ok(savedUserDto, UserResponseMessages.UserCreatedSuccess);
            }
            catch (Exception ex)
            {
                return BadRequest<UserDto>(UserResponseMessages.ErrorOccurred, new List<string> { ex.Message });
            }
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmailAndSetPassword([FromBody] ConfirmEmailDto confirmEmailDto)
        {

            if (confirmEmailDto == null)
            {
                return BadRequest<string>("Invalid input data.");
            }

            try
            {
                User user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
                if (user == null)
                {
                    return BadRequest<string>("Invalid email.");
                }

                // Confirm the email using the token
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);
                if (!result.Succeeded)
                {
                    return BadRequest<string>(AuthResponseMessages.EmailConfirmationFailed);
                }

                // Set the user's new password
                IdentityResult passwordResult = await _userManager.RemovePasswordAsync(user);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest<string>("Failed to remove existing password.");
                }

                IdentityResult setPasswordResult = await _userManager.AddPasswordAsync(user, confirmEmailDto.Password);
                if (!setPasswordResult.Succeeded)
                {
                    return BadRequest<string>("Failed to set new password.");
                }

                return Ok<string>("Email confirmed and password set successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest<string>($"Error: {ex.Message}");
            }
        }


    }
}

using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration config) 
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null) 
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid email"
                    };
                }
                var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (!result)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid password"
                    };
                }
                return new LoginResponse()
                {
                    Success = true,
                    Message = "login Success",
                    AccessToken = await GenerateAccessToken(user)
                };
            }
            catch (Exception ex) 
            {
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Success"
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            
            if (!result.Succeeded) 
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "user creation failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(user, "User");
            return new RegisterResponse()
            {
                Success = true,
                Message = "Success"
            };
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

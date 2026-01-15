
using MaxiShop.Business.InputModels;
using MaxiShop.Business.Services.Interface;
using MaxiShop.Business.ViewModels;
using MaxiShop.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MaxiShop.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private ApplicationUser applicationUser;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            applicationUser = new ApplicationUser();
            
        }

        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            
            applicationUser.FirstName = register.FirstName;
            applicationUser.LastName = register.LastName;
            applicationUser.Email = register.Email;
            applicationUser.UserName = register.Email;

            var result = await _userManager.CreateAsync(applicationUser, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, "CUSTOMER");
            }

            return result.Errors;

        }

        public async Task<object> Login(Login login)
        {
            applicationUser = await _userManager.FindByEmailAsync(login.Email);
            if (applicationUser == null)
            {
                return "Email not found";
            }
            var result = await _signInManager.PasswordSignInAsync(applicationUser, login.Password, true, true);
            bool IsValidateCredential = await _userManager.CheckPasswordAsync(applicationUser, login.Password);
            if (result.Succeeded)
            {
                var token = await GenerateToken();
                return new LoginResponse
                {
                    UserId = applicationUser.Id,
                    Token = token
                };
            }
            else
            {
                if(result.IsLockedOut)
                {
                    return "User is locked out.";
                }
                if(result.IsNotAllowed)
                {
                    return "User is not allowed to sign in.";
                }
                if(IsValidateCredential == false)
                {
                    return "Invalid Password.";
                }
                else
                {
                    return "Login failed.";
                }
            }
        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var signingCredential = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var role = await _userManager.GetRolesAsync(applicationUser);

            var roleClaims = role.Select(x=> new Claim(ClaimTypes.Role, x)).ToList();
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email)
            }.Union<Claim>(roleClaims).ToList();

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSettings:DurationInMinutes"])),
                signingCredentials: signingCredential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

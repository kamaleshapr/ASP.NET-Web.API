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
using TaskManagement.Business.Services.Interface;
using TaskManagement.Business.Utils;
using TaskManagement.Data.Utils;

namespace TaskManagement.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ApplicationUser? _applicationUser;
        private IConfiguration _config;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }
        public async Task<IEnumerable<IdentityError>> Register(RegisterInput registerInput)
        {
            _applicationUser = new ApplicationUser();
            _applicationUser.FirstName = registerInput.FirstName;
            _applicationUser.LastName = registerInput.LastName;
            _applicationUser.Email = registerInput.Email;
            _applicationUser.UserName = registerInput.Email;
            _applicationUser.EmployeeId = registerInput.EmployeeId;
            var result = await _userManager.CreateAsync(_applicationUser, registerInput.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_applicationUser, "DEVELOPER");
            }
            return result.Errors;
        }
        public async Task<object> Login(LoginInput loginInput)
        {
            _applicationUser = await _userManager.FindByEmailAsync(loginInput.Email);
            if(_applicationUser == null)
            {
                return "Email not found";
            }
            var result = await _signInManager.PasswordSignInAsync(_applicationUser, loginInput.Password, true, true);
            if (result.Succeeded)
            {
                var token = await GenerateToken();
                return new LoginResponse() { UserId = _applicationUser.EmployeeId, Token = token };
            }
            else
            {
                return "Invalid Email or Password";
            }
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var signingCredential = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(_applicationUser);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, _applicationUser.Email),
                new Claim("EmployeeId", _applicationUser.EmployeeId.ToString())
            };
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: signingCredential,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSettings:DurationInMinutes"]))
                
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

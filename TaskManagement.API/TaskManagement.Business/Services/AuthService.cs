using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailService _emailService;
        private ApplicationUser _applicationUser;
        private IConfiguration _config;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _emailService = emailService;
        }
        public async Task<IEnumerable<IdentityError>> Register(RegisterInput registerInput)
        {
            _applicationUser = new ApplicationUser();
            _applicationUser.FirstName = registerInput.FirstName;
            _applicationUser.LastName = registerInput.LastName;
            _applicationUser.Email = registerInput.Email;
            _applicationUser.UserName = registerInput.Email;
            if(registerInput.EmployeeId.HasValue)
            {
                _applicationUser.EmployeeId = registerInput.EmployeeId;
            }
            else
            {
                // Generate a new Employee record if id not provided or exist
            }
            var result = await _userManager.CreateAsync(_applicationUser, registerInput.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_applicationUser, "DEVELOPER");
                var token = await GenerateEmailConfirmationTokenAsync(_applicationUser);
                var baseUrl = _config["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                var confirmationLink = $"{baseUrl}user/Confirm-Email?userId={_applicationUser.Id}&token={token}";
                await _emailService.SendRegistrationConfirmationEmailAsync(_applicationUser.Email, _applicationUser.FirstName, confirmationLink);
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

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            if (userId == string.Empty || string.IsNullOrEmpty(token))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid token or user ID." });
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                // Uncomment for on real deployment
                //var baseUrl = _config["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                //var loginLink = $"{baseUrl}/user/login";
                //await _emailService.SendAccountCreatedEmailAsync(user.Email!, user.FirstName!, loginLink);
            }
            return result;
        }

        public Task SendEmailConfirmationAsync(string email)
        {
            // To be implemented while optimizing email functionality
            throw new NotImplementedException();
        }

        private async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return encodedToken;
        }
    }
}

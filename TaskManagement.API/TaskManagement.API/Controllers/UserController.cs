using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Business.Services.Interface;
using TaskManagement.Business.Utils;

namespace TaskManagement.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterInput registerInput)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var results = await _authService.Register(registerInput);
            if(results.Any())
            {
                return BadRequest(results);
            }
            return Created("", results);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("login")]
        public async Task<IActionResult> Login(LoginInput loginInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var results = await _authService.Login(loginInput);
            if (results is string)
            {
                return BadRequest(results);
            }
            return Ok(results);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // this method should not be exposed to user, but for demo purpose only we are exposing on swagger.
            // ignore this for real deployment.
            var results = await _authService.ConfirmEmailAsync(userId, token);
            if (!results.Succeeded || results.Errors.Any())
            {
                return BadRequest(results.Errors);
            }
            return Ok("Registration Completed & Email Confirmed. You can proceed login with your credentials");
        }
    }
}

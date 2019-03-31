using dotnet_core_auth_api.Data.Entities;
using dotnet_core_auth_api.DataContracts.Requests;
using dotnet_core_auth_api.DataContracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_core_auth_api.Controllers
{
    [Route("api/auth")]
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AccountsController(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    Email = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    switch (request.RoleId)
                    {
                        case 1:
                            await _userManager.AddToRoleAsync(user, "Admin");
                            break;
                        default:
                            await _userManager.AddToRoleAsync(user, "Customer");
                            break;
                    }

                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var signedUser = await _userManager.FindByEmailAsync(request.Email);
            var arePasswordsEqual = await _signInManager.UserManager.CheckPasswordAsync(signedUser, request.Password);

            if(arePasswordsEqual)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["ValidIssuer"],
                    audience: _configuration["ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Login failed.");
        }

        [HttpGet("me")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetUserInfo([FromQuery] GetUserInfoRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var userRoles = await _userManager.GetRolesAsync(user);

            var result = new UserInfoResponseDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Role = userRoles.FirstOrDefault()
            };

            return Ok(result);
        }
    }
}
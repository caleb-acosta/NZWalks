using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repositories;

namespace NZWalksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO userRegister) {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = userRegister.Username,
                Email = userRegister.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, userRegister.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (userRegister.Roles != null && userRegister.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, userRegister.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest) {
            IdentityUser? user = await userManager.FindByEmailAsync(loginRequest.Username);

            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    IList<string>? roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        string jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        LoginResponseDTO response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Incorrect username or password");
        }
    }
}

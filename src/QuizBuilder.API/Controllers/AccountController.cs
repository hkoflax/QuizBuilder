using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizBuilder.Services;
using QuizBuilder.Settings;
using QuizBuilder.API.DTOs.Account;
using System.IdentityModel.Tokens.Jwt;
using QuizBuilder.Domain;

namespace QuizBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly TokenSettings _options;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IOptions<TokenSettings> options)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _options = options?.Value ?? 
                new TokenSettings { TokenValidityInMinutes = 60, RefreshTokenValidityInDays = 1, TokenKey = "DefaultKey" };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return await CreateUserObjectAsync(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.UserName))
            {
                return BadRequest($"Username '{registerDto.UserName}' is already taken");
            }

            if (await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest($"Email '{registerDto.Email}' is already taken");
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return await CreateUserObjectAsync(user);
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken(TokenModelDto tokenModelDto)
        {
            if (tokenModelDto is null
                || string.IsNullOrWhiteSpace(tokenModelDto.AccessToken)
                || string.IsNullOrWhiteSpace(tokenModelDto.RefreshToken))
            {
                return BadRequest("Invalid token information in request");
            }

            var accessToken = tokenModelDto.AccessToken;
            var refreshToken = tokenModelDto.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            return await CreateUserObjectAsync(user);
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")] // TODO: Secure this with a password to not allow evryone to do so.
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet]
        [Route("{username}/info")]
        public async Task<IActionResult> GetUserInfo(string username = null)
        {
            username = string.IsNullOrWhiteSpace(username) ? User.Identity.Name : username.Trim();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");
            return Ok(new
            {
                user.Email,
                user.UserName,
                user.DisplayName
            });
        }

        private async Task<UserDto> CreateUserObjectAsync(AppUser user)
        {
            var token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_options.RefreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            return new UserDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                UserName = user.UserName,
                DisplayName = user.DisplayName
            };
        }
    }
}

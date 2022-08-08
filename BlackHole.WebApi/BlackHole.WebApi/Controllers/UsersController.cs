using BlackHole.DAL;
using BlackHole.Login;
using BlackHole.WebApi.Dto;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlackHole.WebApi.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly DataRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITwitterAuthService _twitterAuthService;
        private readonly IGoogleAuthService _googleAuthService;

        public UsersController(
            ITwitterAuthService twitterAuthService,
            IGoogleAuthService googleAuthService,
            IConfiguration configuration)
        {
            _userRepository = new DataRepository<User>(new BHDataContext());
            _configuration = configuration;
            _twitterAuthService = twitterAuthService;
            _googleAuthService = googleAuthService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpUser([FromBody] SignUpUserDto userDto)
        {
            var users = await _userRepository.GetAll();
            if (users.FirstOrDefault(x => x.Mail == userDto.Mail) is not null)
            {
                return BadRequest("User with this email already exists.");
            }

            if (userDto.Password != userDto.RepeatPassword)
            {
                return BadRequest("Repeated password must be equal to password.");
            }

            User user = new()
            {
                Mail = userDto.Mail,
                Password = userDto.Password,
                Role = "user",
            };

            await _userRepository.CreateAsync(user);

            return Ok(user);
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogInUser([FromBody] LogInUserDto userDto)
        {
            var user = (await _userRepository.GetAll()).FirstOrDefault(u => u.Mail == userDto.Mail);
            if (user is null || user.Password != userDto.Password)
            {
                return Unauthorized("Email or password is incorrect.");
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Mail),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleSignIn(GoogleSignInDto userDto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(userDto.IdToken, new GoogleJsonWebSignature.ValidationSettings());
                var user = await _googleAuthService.Authenticate(payload);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Mail),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("twitter")]
        public async Task<IActionResult> TwitterSignIn([FromQuery] string oauth_token, [FromQuery] string oauth_verifier)
        {
            try
            {
                await _twitterAuthService.GetAccessToken(oauth_token, oauth_verifier);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("twitter-request")]
        public async Task<IActionResult> TwitterRequest()
        {
            try
            {
                var requestToken = await _twitterAuthService.GetRequestToken();
                return Ok(requestToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        { 
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}

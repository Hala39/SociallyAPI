using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService token, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<AccountDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("Username already taken");
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user!");
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AccountDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }

        private AccountDto CreateUserObject(AppUser user)
        {
            var mappedPhotos = user.Photos.Select(p => _mapper.Map<PhotoDto>(p)).ToList();


            return new AccountDto
            {
                Education = user.Education,
                Bio = user.Bio,
                Work = user.Work,
                Address = user.Address,
                UserName = user.UserName,
                Token = _token.CreateToken(user),
                Photos = mappedPhotos,
                PPUrl = user.Photos.FirstOrDefault(p => p.IsMain == true).Url
            };
        }

        [HttpGet("checkEmail")]
        public async Task<bool> EmailExists([FromQuery] string email)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == email))
            {
                return true;
            }

            return false;
        }

        [HttpGet("checkUsername")]
        public async Task<bool> UsernameExists([FromQuery] string username)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == username))
            {
                return true;
            }

            return false;
        }

        [HttpGet("checkPass")]
        public async Task<bool> CheckPassword([FromQuery] string email, string password)
        {
            if (await EmailExists(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
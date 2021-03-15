using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepo(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        public string GetUserById() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<AppUserDto> GetMyAccount()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetUserByUserName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> UpdateUserInfo(AppUserDto newInfo)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserById());

            if (user == null)
            {
                return null;
            }

            if (newInfo.Education != null)
            {
                user.Education = newInfo.Education;
            }
            if (newInfo.Work != null)
            {
                user.Work = newInfo.Work;
            }
            if (newInfo.Address != null)
            {
                user.Address = newInfo.Address;
            }
            if (newInfo.Bio != null)
            {
                user.Bio = newInfo.Bio;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<AppUserDto>(user);

        }
    }
}
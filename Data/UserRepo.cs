using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserRepo(DataContext context, IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppUserDto> GetMyAccount()
        {
            var user = await _context.Users
                .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, new {currentUserName = _userService.GetUserName()})
                .FirstOrDefaultAsync(u => u.UserName == _userService.GetUserName());
             return user;
        }

        public async Task<AppUserDto> GetUserByUserName(string userName)
        {
            var user = await _context.Users
                .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, new {currentUserName = _userService.GetUserName()})
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return user;

        }

        public async Task<AppUserDto> UpdateUserInfo(AppUserDto newInfo)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == _userService.GetUserName());

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
            
            var result = await _context.SaveChangesAsync() > 0;

            var mapped = await _context.Users
                .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, new {currentUserName = _userService.GetUserName()})
                .FirstOrDefaultAsync(u => u.UserName == _userService.GetUserName());

            if (result) return mapped;

            return null;
        }
    }
}
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System;
using System.Linq;
using API.Helpers;
using API.Entities;

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

        public async Task<AppUserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<AppUserDto>(user);
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
        
        public async Task<PagedList<AppUserDto>> SearchUsers(UserParams userParams)
        {   
            var query = _context.Users
            .ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, new {currentUserName = _userService.GetUserName()})
            .Where(u => u.UserName.ToLower().Contains(userParams.SearchString.ToLower())
                    || u.Bio.ToLower().Contains(userParams.SearchString.ToLower())).AsNoTracking();
            

            return await PagedList<AppUserDto>.CreateAsync(query, userParams.PageNumber, userParams.PageNumber);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Contact>> GetContacts() 
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());
            var followings = await _context.UserFollowings.Where(u => u.ObserverId == user.Id).Select(u => u.Target).ToListAsync();
            var followers = await _context.UserFollowings.Where(u => u.TargetId == user.Id).Select(u => u.Observer).ToListAsync();
            var contacts = new List<AppUser>();

            if (followings != null) 
            {
                foreach (var item in followings)
                {
                    if (!contacts.Contains(item))
                    {
                        contacts.Add(item); 
                    }
                }
            }

            if (followers != null) 
            {
                foreach (var item in followers)
                {
                    if (!contacts.Contains(item))
                    {
                        contacts.Add(item); 
                    }
                }
            }

            contacts.Remove(user);

            var mapped = contacts.Select(u => _mapper.Map<Contact>(u)).ToList();

            return mapped;
            
        }
    
        public async Task<AppUser> GetUserByUsernameAsync(string userName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == userName);
        }
    }

}
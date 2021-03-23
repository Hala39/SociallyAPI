using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepo : IPhotoRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public PhotoRepo(DataContext context, IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<PostDto>> GetPosts()
        {
            var posts = await _context.Photos
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider, new { currentUserName = _userService.GetUserName()})
            .OrderBy(p => p.Following)
            .OrderBy(p => p.Time)
            .AsNoTracking()
            .ToListAsync();

            return posts;
        }
    }
}

// .Where(u => u.UserName.ToLower().Contains(userParams.SearchString.ToLower())
//                     || u.Bio.ToLower().Contains(userParams.SearchString.ToLower()))
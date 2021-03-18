using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
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

        public async Task<List<PhotoDto>> GetPhotos()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());
            var photos = await _context.Photos
                .ProjectTo<PhotoDto>(_mapper.ConfigurationProvider, new { currentUserName = user.UserName})
                .ToListAsync();

            return photos;
        }
    }
}
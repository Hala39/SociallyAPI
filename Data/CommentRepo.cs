using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CommentRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public CommentRepo(DataContext context, IMapper mapper, IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;

        }

        public async Task<CommentDto> AddComment(int photoId, string body)
        {
            var photo = await _context.Photos.FindAsync(photoId);

            if(photo == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

            var comment = new Comment
            {
                AppUser = user,
                Photo = photo,
                Body = body
            };

            photo.Comments.Add(comment);

            var success = await _context.SaveChangesAsync() > 0;

            if(!success) return null;

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<List<CommentDto>> List(int photoId)
        {
            var photo = await _context.Photos.FindAsync(photoId);

            if(photo == null) return null;
            
            var comments = await _context.Comments
                .Where(c => c.Photo.Id == photoId)
                .OrderBy(c => c.Time)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return comments;
        }
    }
}
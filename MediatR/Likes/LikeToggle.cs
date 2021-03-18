using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.MediatR.Likes
{
    public class LikeToggle
    {
        public class Command : IRequest<Unit>
        {
            public int PhotoId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserService _userService;
            private readonly DataContext _context;
            public Handler(DataContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var liker = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userService.GetUserName());
                var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == request.PhotoId);

                if (photo == null) return Unit.Value;
                
                var like = await _context.Likes.FindAsync(liker.Id, photo.Id);

                if (like == null)
                {
                    like = new Like
                    {
                        AppUser = liker,
                        Photo = photo
                    };

                    await _context.Likes.AddAsync(like);
                }
                else 
                {
                    _context.Likes.Remove(like);
                }

                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    
    }
}           
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.MediatR.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Unit>
        {
            public string TargetUserName { get; set; }

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
                var observer = await _context.Users.FirstOrDefaultAsync(u => 
                    u.Id == _userService.GetUserId());

                var target = await _context.Users.FirstOrDefaultAsync(u 
                    => u.UserName == request.TargetUserName);

                if(target == null) return Unit.Value;

                var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                if (following == null) 
                {
                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };

                    await _context.UserFollowings.AddAsync(following);
                }
                else 
                {
                     _context.UserFollowings.Remove(following);
                }

                await _context.SaveChangesAsync();

                return Unit.Value;

            }
        }
    }
}
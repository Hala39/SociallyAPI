using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.MediatR.Comments
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int CommentId { get; set; }

        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserService _userService;
            public Handler(DataContext context, IUserService userService)
            {
                _userService = userService;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == request.CommentId);

                if (comment == null) return Unit.Value;

                if (comment.AppUser.Id == user.Id) {
                     _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
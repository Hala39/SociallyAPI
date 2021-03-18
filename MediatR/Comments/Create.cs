using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.MediatR.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public int PhotoId { get; set; }
            public string Body { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;
            public Handler(DataContext context, IMapper mapper, IUserService userService)
            {
                _userService = userService;
                _mapper = mapper;
                _context = context;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == request.PhotoId);

                if (photo == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userService.GetUserId());

                var comment = new Comment
                {
                    AppUser = user,
                    Photo = photo,
                    Body = request.Body
                };

                photo.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if(!success) return null;

                return _mapper.Map<CommentDto>(comment);
            }
        }
    }
}
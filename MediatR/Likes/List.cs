using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.MediatR.Likes
{
    public class List
    {
        public class Query : IRequest<List<string>>
        {
            public int PhotoId { get; set; }

        }

        public class Handler : IRequestHandler<Query, List<string>>
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

            public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var likers = await _context.Likes.Where(x => x.Photo.Id == request.PhotoId)
                            .Select(u => u.AppUser).ProjectTo<AppUserDto>(_mapper.ConfigurationProvider,
                                new { currentUserName = _userService.GetUserName() }).Select(u => u.UserName).ToListAsync();

                return likers;
            }
        }
    }
}
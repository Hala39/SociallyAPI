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

namespace API.MediatR.Followers
{
    public class List
    {
        public class Query : IRequest<List<AppUserDto>> 
        {
            public string Predicate { get; set; }
            public string UserName { get; set; }
            
        }

        public class Handler : IRequestHandler<Query, List<AppUserDto>>
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

            public async Task<List<AppUserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = new List<AppUserDto>();

                switch (request.Predicate)
                {
                    case "followers":
                        users = await _context.UserFollowings.Where(x => x.Target.UserName == request.UserName)
                            .Select(u => u.Observer).ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, 
                                new { currentUserName = _userService.GetUserName()}).ToListAsync();
                    break;
                    case "following":
                        users = await _context.UserFollowings.Where(x => x.Observer.UserName == request.UserName)
                            .Select(u => u.Target).ProjectTo<AppUserDto>(_mapper.ConfigurationProvider, 
                                new { currentUserName = _userService.GetUserName()}).ToListAsync();
                    break;
                }

                return users;
            }
        }
    }
}
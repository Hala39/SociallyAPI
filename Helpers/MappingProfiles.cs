using System.Linq;
using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUserName = null;

            CreateMap<AppUser, AppUserDto>()
                .ForMember(d => d.PPUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain == true).Url))
                .ForMember(d => d.CoverUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsCover == true).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(d => d.Followers.Any(x => x.Observer.UserName == currentUserName)));

            CreateMap<AppUser, AccountDto>()
                .ForMember(d => d.PPUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain == true).Url))
                .ForMember(d => d.CoverUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsCover == true).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count));

            CreateMap<Photo, PhotoDto>()
                .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUserName)));
                
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain == true).Url));

        }
    }
}
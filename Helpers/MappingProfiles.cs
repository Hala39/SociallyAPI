using System;
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
                .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUserName)))
                .ForMember(d => d.Liked, o => o.MapFrom(d => d.Likers.Any(x => x.AppUser.UserName == currentUserName)))
                .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likers.Count));

            CreateMap<Photo, PostDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.UserPPUrl, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain == true).Url))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUserName)))
                .ForMember(d => d.Liked, o => o.MapFrom(d => d.Likers.Any(x => x.AppUser.UserName == currentUserName)))
                .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likers.Count));
                
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain == true).Url));

            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain == true).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain == true).Url));

            CreateMap<AppUser, Contact>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain == true).Url));

            CreateMap<UserFollowing, Contact>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Target.UserName));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d ,DateTimeKind.Utc));

        }
    }
}
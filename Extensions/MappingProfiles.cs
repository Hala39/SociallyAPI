using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, AppUserDto>();
            CreateMap<Photo, PhotoDto>();
        }
    }
}
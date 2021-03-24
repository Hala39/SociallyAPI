using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        Task<AppUserDto> GetUserByIdAsync(string id);
        Task<AppUserDto> GetUserByUserName(string userName);
        Task<AppUserDto> UpdateUserInfo(AppUserDto newInfo);
        Task<AppUser> GetUserByUsernameAsync(string userName);
        Task<PagedList<AppUserDto>> SearchUsers(UserParams userParams);
        Task<List<Contact>> GetContacts();
        Task<AppUserDto> GetMyAccount();
        Task<bool> SaveAllAsync();
    }
}
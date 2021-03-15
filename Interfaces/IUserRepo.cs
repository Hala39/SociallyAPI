using System.Threading.Tasks;
using API.Dtos;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        Task<AppUserDto> GetUserByUserName(string userName);
        Task<AppUserDto> UpdateUserInfo(AppUserDto newInfo);
        Task<AppUserDto> GetMyAccount();
    }
}
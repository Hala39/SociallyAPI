using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPhotoRepo
    {
        Task<List<PostDto>> GetPosts();
    }
}
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoRepo
    {
        Task<PagedList<PhotoDto>> GetPhotos(PhotoParams photoParams);
    }
}
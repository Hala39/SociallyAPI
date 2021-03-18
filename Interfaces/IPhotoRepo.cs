using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;

namespace API.Interfaces
{
    public interface IPhotoRepo
    {
         Task<List<PhotoDto>> GetPhotos();
    }
}
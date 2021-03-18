using System.Threading.Tasks;
using API.MediatR.Likes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikeController : BaseAPiController
    {
        [HttpPost("{photoId}")]
        public async Task<IActionResult> Like(int photoId)
        {
            return Ok(await Mediator.Send( new LikeToggle.Command{ PhotoId = photoId }));
        }

        [HttpGet]
        public async Task<IActionResult> GetLikers(int photoId)
        {
            return Ok(await Mediator.Send( new List.Query{PhotoId = photoId }));
        }
    }
}
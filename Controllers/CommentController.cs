using System.Threading.Tasks;
using API.Dtos;
using API.MediatR.Comments;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommentController : BaseAPiController
    {
         [HttpPost]
        public async Task<IActionResult> Comment(CreateCommentDto createCommentDto)
        {
            return Ok(await Mediator.Send( new Create.Command{ PhotoId = createCommentDto.PhotoId, Body = createCommentDto.Body }));
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int photoId)
        {
            return Ok(await Mediator.Send( new List.Query{ PhotoId = photoId }));
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            return Ok(await Mediator.Send( new Delete.Command{ CommentId = commentId}));
        }
    }
}
using System.Threading.Tasks;
using API.MediatR.Followers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseAPiController
    {
        [HttpPost("{userName}")]
        public async Task<ActionResult<Unit>> Follow(string userName)
        {
            return Ok(await Mediator.Send(new FollowToggle.Command{TargetUserName = userName}));
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<Unit>> GetFollowings(string userName, string predicate)
        {
            return Ok(await Mediator.Send(new List.Query{UserName = userName, Predicate = predicate}));
        }

    }
}
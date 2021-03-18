using System.Threading.Tasks;
using API.MediatR.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class CommentHub : Hub
    {
        private readonly IMediator _mediator;
        public CommentHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.PhotoId.ToString())
                .SendAsync("ReceiveComment", comment);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var photoId = httpContext.Request.Query["photoId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, photoId);
            var result = await _mediator.Send(new List.Query{PhotoId = int.Parse(photoId)});
            await Clients.Caller.SendAsync("LoadComments", result);
        }
    }
}
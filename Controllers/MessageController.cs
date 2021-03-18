using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MessageController : BaseAPiController
    {
        private readonly IMessageRepo _messageRepo;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageController(DataContext context, IMessageRepo messageRepo, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _messageRepo = messageRepo;

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userName = User.GetUsername();

            if (userName == createMessageDto.RecipientUserName.ToLower())
            {
                return BadRequest("You can not send a message to yourself");
            }

            var sender = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.UserName == createMessageDto.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = createMessageDto.Content
            };

            _messageRepo.AddMessage(message);

            if (await _messageRepo.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to Send Message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();

            var messages = await _messageRepo.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("Thread/{userName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName)
        {
            var currentUserName = User.GetUsername();

            return Ok(await _messageRepo.GetMessageThread(currentUserName, userName));
        }
    }
}


// .cs

using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Extensions;
using APIDatingApp.Helpers;
using APIDatingApp.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIDatingApp.Controllers
{
    public class MessagesController : BaseApiController
    {
        // private readonly IUserRepository _userRepository;
        // private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _uow;

        // public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        public MessagesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            // _userRepository = userRepository;
            // _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO){
            var username = User.GetUsername();

            if (username == createMessageDTO.RecipientUserName.ToLower())
                return BadRequest("You cannot send messages to yourself");

            // var sender = await _userRepository.GetUserByUserNameAsync(username);
            var sender = await _uow.UserRepository.GetUserByUserNameAsync(username);
            // var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDTO.RecipientUserName);
            var recipient = await _uow.UserRepository.GetUserByUserNameAsync(createMessageDTO.RecipientUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDTO.Content
            };

            // _messageRepository.AddMessage(message);
            _uow.MessageRepository.AddMessage(message);

            // if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDTO>(message));
            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDTO>(message));

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParamas messageParams)
        {
            messageParams.Username = User.GetUsername();

            // var messages = await _messageRepository.GetMessagesForUser(messageParams);
            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

            return messages;

        }

/*
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            // return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
            return Ok(await _uow.MessageRepository.GetMessageThread(currentUsername, username));
        }
        */

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            // var message = await _messageRepository.GetMessage(id);
            var message = await _uow.MessageRepository.GetMessage(id);

            if (message.SenderUserName != username && message.RecipientUserName != username) return Unauthorized();

            if (message.SenderUserName == username) message.SenderDeleted = true;
            if (message.RecipientUserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                // _messageRepository.DeleteMessage(message);
                _uow.MessageRepository.DeleteMessage(message);
            }

            // if (await _messageRepository.SaveAllAsync()) return Ok();
            if (await _uow.Complete()) return Ok();

            return BadRequest("Problem deleting the message");
        }
    }
}
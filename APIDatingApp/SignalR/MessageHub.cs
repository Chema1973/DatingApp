using APIDatingApp.DTOs;
using APIDatingApp.Entities;
using APIDatingApp.Extensions;
using APIDatingApp.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace APIDatingApp.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        // private readonly IMessageRepository _messageRepository;
        // private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _pressenceHub;
        /// --> Para avisar a un usuario que le ha llegado un mensaje desde otro usuario
        private readonly IUnitOfWork _uow;

        // public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper,
        public MessageHub(IUnitOfWork uow, IMapper mapper,
            IHubContext<PresenceHub> pressenceHub){
            // _messageRepository = messageRepository;
            // _userRepository = userRepository;
            _uow = uow;
            _mapper = mapper;
            _pressenceHub = pressenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            // var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            var messages = await _uow.MessageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

            if (_uow.HasChanges()) await _uow.Complete();


            // await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(CreateMessageDTO createMessageDto)
        {
            
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUserName.ToLower())
                throw new HubException("You cannot send messages to yourself");

            // var sender = await _userRepository.GetUserByUserNameAsync(username);
            var sender = await _uow.UserRepository.GetUserByUserNameAsync(username);
            // var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);
            var recipient = await _uow.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            // var group = await _messageRepository.GetMessageGroup(groupName);
            var group = await _uow.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            } else {
                /// Para avisar a un usuario que le ha llegado un mensaje desde otro usuario
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null){
                    await _pressenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", 
                    new {username = sender.UserName, knownAs = sender.KnownAs});
                }
            }

            // _messageRepository.AddMessage(message);
            _uow.MessageRepository.AddMessage(message);

            // if (await _messageRepository.SaveAllAsync()){
            if (await _uow.Complete()){
                // var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Groups(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            } 
        }


        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }


        private async Task<Group> AddToGroup(string groupName) {
            // var group = await _messageRepository.GetMessageGroup(groupName);
            var group = await _uow.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null){
                group = new Group(groupName);
                // _messageRepository.AddGroup(group);
                _uow.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            // if (await _messageRepository.SaveAllAsync()) return group;
            if (await _uow.Complete()) return group;

            throw new HubException("Faild to add to Group");

        }

        private async Task<Group> RemoveFromMessageGroup(){
            // var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
            var group = await _uow.MessageRepository.GetGroupForConnection(Context.ConnectionId);

            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            // _messageRepository.RemoveConnection(connection);
            _uow.MessageRepository.RemoveConnection(connection);
            // if (await _messageRepository.SaveAllAsync()) return group;
            if (await _uow.Complete()) return group;

            throw new HubException("Faild to remove from Group");
        }
    }
}
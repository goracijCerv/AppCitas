using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Extensions;
using Apcitas.WebService.Helpers;
using Apcitas.WebService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apcitas.WebService.Controllers;

[Authorize]
public class MessagesController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto messageCreateDto)
    {
        var username = User.GetUsername();

        if (username.ToLower().Equals(messageCreateDto.RecipientUsername.ToLower()))
            return BadRequest("you can´t send messages to yourself");

        var sender = await _userRepository.GetUserByUsernameAsync(username);
        var recipient = await _userRepository.GetUserByUsernameAsync(messageCreateDto.RecipientUsername);

        if (recipient == null) return NotFound();

        var message = new Message
        {
            Sender = sender,
            Recipent = recipient,
            SenderUsername = sender.UserName,
            RecipentUsername = recipient.UserName,
            Content = messageCreateDto.Content
        };

        _messageRepository.AddMessage(message);

        if (await _messageRepository.SaveAllAsync())
            return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Faild to send the message");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.UserName = User.GetUsername();
        var messages = await _messageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var curretnUsername = User.GetUsername();
        return Ok(await _messageRepository.GetMessageThread(curretnUsername, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DelateMessage(int id)
    {
        var username = User.GetUsername();
        var message = await _messageRepository.GetMessage(id);

        if (!message.Sender.UserName.Equals(username) && !message.Recipent.UserName.Equals(username))
            return Unauthorized();

        if (message.Sender.UserName.Equals(username)) message.SenderDelate = true;

        if (message.Recipent.UserName.Equals(username)) message.RecipientDelate = true;

        if (message.SenderDelate && message.RecipientDelate)
            _messageRepository.DelateMessage(message);

        if (await _messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the message");
    }
}

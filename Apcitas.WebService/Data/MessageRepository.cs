using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;
using Apcitas.WebService.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DelateMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
        return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipent)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser( MessageParams messageParams)
    {
        var query = _context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        query = messageParams.Container.ToLower() switch
        {
            "inbox" => query.Where(u => u.Recipent.UserName.Equals(messageParams.UserName) && u.RecipientDelate == false),
            "outbox" => query.Where(u => u.Sender.UserName.Equals(messageParams.UserName) && u.SenderDelate == false),
            _ => query.Where(u => u.Recipent.UserName.Equals(messageParams.UserName) && u.DateRead == null && u.RecipientDelate == false)
        };

        var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

        return await PagedList<MessageDto>
            .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSie);

    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipent).ThenInclude(p => p.Photos)
            .Where(m => m.Recipent.UserName.Equals(currentUsername) && m.RecipientDelate == false
                    && m.Sender.UserName.Equals(recipientUsername)
                    || m.Recipent.UserName.Equals(recipientUsername)
                    && m.Sender.UserName.Equals(currentUsername) && m.SenderDelate == false)  
            .OrderBy(m => m.MessageSent)
            .ToListAsync();

        var unreadMessages = messages
            .Where(m => m.DateRead == null && m.Recipent.UserName.Equals(currentUsername)).ToList();

        if (unreadMessages.Any())
        {
            foreach ( var message in unreadMessages)
            {
                message.DateRead = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

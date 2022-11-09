using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;
using Apcitas.WebService.Interfaces;

namespace Apcitas.WebService.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;

    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public void AddMessage(Message message)
    {
        throw new NotImplementedException();
    }

    public void DelateMessage(Message message)
    {
        throw new NotImplementedException();
    }

    public Task<Message> GetMessage(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<MessageDto>> GetMessagesForUser()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAllAsync()
    {
        throw new NotImplementedException();
    }
}

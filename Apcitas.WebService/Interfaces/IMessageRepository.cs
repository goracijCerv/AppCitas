using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;

namespace Apcitas.WebService.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DelateMessage(Message message);
    Task<Message> GetMessage(int id);
    Task<PagedList<MessageDto>> GetMessagesForUser();
    Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int  recipientId);
    Task<bool> SaveAllAsync();
}

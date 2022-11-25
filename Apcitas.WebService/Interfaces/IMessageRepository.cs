using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;

namespace Apcitas.WebService.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DelateMessage(Message message);
    Task<Message> GetMessage(int id);
    Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUsername);
    Task<bool> SaveAllAsync();
}

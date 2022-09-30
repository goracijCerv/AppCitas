using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;

namespace Apcitas.WebService.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string userName);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<bool> SaveAllAsync();
    void Update(AppUser user);

    Task<MemberDto> GetMemberAsync(string userName);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
}

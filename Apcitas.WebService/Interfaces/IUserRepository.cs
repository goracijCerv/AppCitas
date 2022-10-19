using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;

namespace Apcitas.WebService.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string userName);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<bool> SaveAllAsync();
    void Update(AppUser user);

    Task<MemberDto> GetMemberAsync(string userName);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
}

using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;

namespace Apcitas.WebService.Interfaces;


public interface ILikeRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
    Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    Task<AppUser> GetUserWithLikes(int userId);
}

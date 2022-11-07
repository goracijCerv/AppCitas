using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Helpers;

namespace Apcitas.WebService.Interfaces;


public interface ILikeRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    Task<AppUser> GetUserWithLikes(int userId);
}

using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Interfaces;
using AutoMapper;

namespace Apcitas.WebService.Data;

public class LikesRepository : ILikeRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public LikesRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> GetUserWithLikes(int userId)
    {
        throw new NotImplementedException();
    }
}

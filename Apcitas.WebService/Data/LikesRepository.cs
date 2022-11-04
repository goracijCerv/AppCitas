﻿using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Extensions;
using Apcitas.WebService.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        return await _context.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
        var likes = _context.Likes.AsQueryable();

        if (predicate.ToLower().Equals("liked"))
        {
            likes = likes.Where(like => like.SourceUserId == userId);
            users = likes.Select(like => like.LikedUser);
        }

        if (predicate.ToLower().Equals("likedby"))
        {
            likes = likes.Where(like => like.LikedUserId == userId);
            users = likes.Select(like => like.SourceUser);
        }

        return await users.Select(user => new LikeDto
        {
            Username = user.UserName,
            KnownAs = user.KnowAs,
            Age = user.DateOfBirth.CalculeteAge(),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
            City = user.City,
            Id = user.Id
        }).ToListAsync();

    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}

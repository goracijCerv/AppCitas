using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Extensions;
using Apcitas.WebService.Helpers;
using Apcitas.WebService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Apcitas.WebService.Controllers;

public class LikesController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly ILikeRepository _likeRepository;

    public LikesController(ILikeRepository likeRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _likeRepository = likeRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _userRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName.Equals(username)) return BadRequest("you can´t like yourself");

        var userLike = await _likeRepository.GetUserLike(sourceUserId, likedUser.Id);

        if (userLike != null) return BadRequest("you alredy like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Faild yo like user");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _likeRepository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
        return Ok(users);
    }

}

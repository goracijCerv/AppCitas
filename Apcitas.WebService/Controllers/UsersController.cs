using Apcitas.WebService.Data;
using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Apcitas.WebService.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    //Get api/users
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

         return Ok(users);
    }
    //Get api/usuarios/id 
    [HttpGet("{username}")]
   
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string userName)
    {
        return await _userRepository.GetMemberAsync(userName);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdate)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(username);
        _mapper.Map(memberUpdate, user);

        _userRepository.Update(user);
        if (await _userRepository.SaveAllAsync()) return NoContent();
        
        return BadRequest("Fail to update the user");
    }
}

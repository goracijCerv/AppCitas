using Apcitas.WebService.Data;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    //Get api/users
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
         return Ok(await _userRepository.GetUsersAsync());
    }
    //Get api/usuarios/id 
    [HttpGet("{id}")]
   
    public async Task<ActionResult<AppUser>> GetUserByUserName(string userName)
    {
        return await _userRepository.GetUserByUsernameAsync(userName);
    }
}

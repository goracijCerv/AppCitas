using Apcitas.WebService.Data;
using Apcitas.WebService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    public UserController(DataContext context)
    {
          _context = context;
    }
    //Get api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
         return await _context.Users.ToListAsync();
    }
    //Get api/usuarios/id 
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUsersById(int id)
    {
        return await _context.Users.FindAsync(id); 
    }
}

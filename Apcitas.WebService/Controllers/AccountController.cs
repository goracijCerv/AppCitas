using Apcitas.WebService.Data;
using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Apcitas.WebService.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly iTokenService _iTokenService;
    private readonly IMapper _mapper;


    public AccountController(DataContext context, iTokenService iTokenService, IMapper mapper)
    {
        _context = context;
        _iTokenService = iTokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
            return BadRequest("Username is already taken!");
        using var hmac = new HMACSHA512();

        var user = _mapper.Map<AppUser>(registerDto);
        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordHash = hmac.Key;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto
        {
            UserName = user.UserName,
            Token = _iTokenService.CreateToken(user),
            KnowAs = user.KnowAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username or password");

        using var hac = new HMACSHA512(user.PasswordSalt);

        var computeHash = hac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid username or password");
        }

        return new UserDto
        {
            UserName = user.UserName,
            Token = _iTokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnowAs = user.KnowAs,
            Gender = user.Gender
        };
    }

    #region Private Methods
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
    #endregion
}

﻿using Apcitas.WebService.Data;
using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Extensions;
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
    private readonly IPhotoService _photoService;
    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
    }
    //Get api/users
    [HttpGet]
    
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

         return Ok(users);
    }
    //Get api/usuarios/id 
    [HttpGet("{username}", Name ="GetUser")]
   
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string userName)
    {
        return await _userRepository.GetMemberAsync(userName);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdate)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        _mapper.Map(memberUpdate, user);

        _userRepository.Update(user);
        if (await _userRepository.SaveAllAsync()) return NoContent();
        
        return BadRequest("Fail to update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId= result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);
        if (await _userRepository.SaveAllAsync())
        {
            return CreatedAtRoute(
                 "GetUser",
                 new {username = user.UserName},
                _mapper.Map<PhotoDto>(photo));
        }
        return BadRequest("problem with adding a photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int idphoto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        var photo = user.Photos.FirstOrDefault(x => x.Id == idphoto);

        if (photo == null) return NotFound("This photo not exist");
        
        if (photo.IsMain) return BadRequest("This is already you main photo");

        var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

        if (currentMainPhoto != null) currentMainPhoto.IsMain = false;

        photo.IsMain = true;

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Faild to set main photo");
    }

}

namespace Apcitas.WebService.Helpers;

using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using AutoMapper;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles() {
        CreateMap<AppUser, MemberDto>();
        CreateMap<Photo, PhotoDto>();
    }
        
}

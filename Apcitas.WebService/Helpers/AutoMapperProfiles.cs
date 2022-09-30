namespace Apcitas.WebService.Helpers;

using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using AutoMapper;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles() {
        CreateMap<AppUser, MemberDto>()
            .ForMember(
            dest => dest.PhotoUrl,
            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(
                x=> x.IsMain).Url));
        CreateMap<Photo, PhotoDto>();
    }
        
}

namespace Apcitas.WebService.Helpers;

using Apcitas.WebService.DTOs;
using Apcitas.WebService.Entities;
using Apcitas.WebService.Extensions;
using AutoMapper;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles() {
        CreateMap<AppUser, MemberDto>()
            .ForMember(
            dest => dest.PhotoUrl,
            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(
                x=> x.IsMain).Url))
            .ForMember(
            dest => dest.Age,
            opt => opt.MapFrom(src => src.DateOfBirth.CalculeteAge()));
                
        CreateMap<Photo, PhotoDto>();

        CreateMap<MemberUpdateDto, AppUser>();
    }
        
}

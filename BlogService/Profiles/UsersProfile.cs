using AutoMapper;
using BlogService.Dtos;
using BlogService.Models;

namespace BlogService.Profiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        // Source -> Target
        CreateMap<User, UserReadDto>();
        CreateMap<UserPublishedDto, User>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
}

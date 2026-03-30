using AutoMapper;
using BlogService.Dtos;
using BlogService.Models;

namespace BlogService.Profiles;

public class BlogsProfile : Profile
{
    public BlogsProfile()
    {
        // Source -> Target
        CreateMap<Blog, BlogReadDto>();
        CreateMap<BlogCreateDto, Blog>();
    }
}

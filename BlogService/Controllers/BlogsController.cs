using AutoMapper;
using BlogService.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers;

[Route("b/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly IBlogRepo _repo;
    private readonly IMapper _mapper;

    public BlogsController(IBlogRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
}

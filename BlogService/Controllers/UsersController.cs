using AutoMapper;
using BlogService.Data;
using BlogService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers;

[Route("b/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepo _repo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserReadDto>> GetUsers()
    {
        var userItems = _repo.GetAllUsers();

        return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
    }
}

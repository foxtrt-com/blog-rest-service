using System;
using System.Text.Json;
using AutoMapper;
using BlogService.Data;
using BlogService.Dtos;
using BlogService.Models;

namespace BlogService.EventProcessing;

public class UserEventsProcessor : IUserEventsProcessor
{
    private IServiceScopeFactory _scopeFactory;
    private IMapper _mapper;

    public UserEventsProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void AddUser(string userPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IUserRepo>();

            var userPublishedDto = JsonSerializer.Deserialize<UserPublishedDto>(userPublishedMessage);

            try
            {
                var user = _mapper.Map<User>(userPublishedDto);
                if (!repo.ExternalUserExists(user.ExternalId))
                {
                    repo.CreateUser(user);
                    repo.SaveChanges();
                }
                else
                {
                    Console.WriteLine("User already exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not add user to Db: {ex}");
            }
        }
    }
}

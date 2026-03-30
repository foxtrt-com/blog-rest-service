using BlogService.Models;

namespace BlogService.Data;

public interface IUserRepo
{
    bool SaveChanges();
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    void CreateUser(User user);
    bool ExternalUserExists(int externalUserId);
    public User GetUserByExternalId(int externalUserId);
}

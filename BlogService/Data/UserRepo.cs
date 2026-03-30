using BlogService.Models;

namespace BlogService.Data;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }

    public void CreateUser(User user)
    {
        // Check if user is null, throw exception if it is
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        // Add user to Db
        _context.Users.Add(user);
    }

    public bool ExternalUserExists(int externalUserId)
    {
        return _context.Users.Any(u => u.ExternalId == externalUserId);
    }

    public IEnumerable<User> GetAllUsers()
    {
        // Return all users from Db
        return _context.Users.ToList();
    }

    public User GetUserById(int id)
    {
        // Return user with matching id from Db
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User GetUserByExternalId(int externalUserId)
    {
        // Return user with matching id from Db
        return _context.Users.FirstOrDefault(u => u.ExternalId == externalUserId);
    }

    public bool SaveChanges()
    {
        // Save changes to Db, return true if changes > 0 were saved
        return _context.SaveChanges() > 0;
    }
}

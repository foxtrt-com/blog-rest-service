using BlogService.Models;

namespace BlogService.Data;

public class BlogRepo : IBlogRepo
{
    private readonly AppDbContext _context;

    public BlogRepo(AppDbContext context)
    {
        _context = context;
    }

    public void CreateBlog(Blog blog)
    {
        if (blog == null)
        {
            throw new ArgumentNullException(nameof(blog));
        }

        _context.Blogs.Add(blog);
    }

    public IEnumerable<Blog> GetAllBlogs()
    {
        return _context.Blogs.ToList();
    }

    public Blog GetBlogById(int id)
    {
        return _context.Blogs.FirstOrDefault(b => b.Id == id);
    }

    public IEnumerable<Blog> GetBlogsForUser(int userId)
    {
        return _context.Blogs
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.PublishedAt);
    }

    public bool SaveChanges()
    {
        // Save changes to Db, return true if changes > 0 were saved
        return _context.SaveChanges() > 0;
    }
}

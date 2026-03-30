using BlogService.Models;

namespace BlogService.Data;

public interface IBlogRepo
{
    bool SaveChanges();
    IEnumerable<Blog> GetAllBlogs();
    IEnumerable<Blog> GetBlogsForUser(int userId);
    Blog GetBlogById(int id);
    void CreateBlog(Blog blog);

}

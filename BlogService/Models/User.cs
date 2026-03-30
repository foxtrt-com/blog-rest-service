using System.ComponentModel.DataAnnotations;

namespace BlogService.Models;

public class User
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int ExternalId { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    public ICollection<Blog> Blogs { get; set; } = [];
}

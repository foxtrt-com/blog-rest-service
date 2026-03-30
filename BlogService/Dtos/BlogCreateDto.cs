using System.ComponentModel.DataAnnotations;

namespace BlogService.Dtos;

public class BlogCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;
}

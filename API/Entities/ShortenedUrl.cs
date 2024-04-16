using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class ShortenedUrl
{
    [Key]
    public int Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedData { get; set; }
    public string CreatedBy { get; set; }
}
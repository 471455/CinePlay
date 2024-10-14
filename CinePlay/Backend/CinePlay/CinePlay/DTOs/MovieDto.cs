using MyMDb.Models.Base;

namespace MyMDb.DTOs
{
    public class MovieDto : MediaDto
    {
        public string? VideoPath { get; set; }
    }
}

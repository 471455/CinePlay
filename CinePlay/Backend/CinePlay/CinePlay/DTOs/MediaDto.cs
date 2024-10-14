using MyMDb.Models.Base;

namespace MyMDb.DTOs
{
    public class MediaDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? MediaType { get; set; }
    }
}

using MyMDb.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MyMDb.DTOs
{
    public class EpisodeDto : MediaDto
    {
        public Guid? SeriesId { get; set; }
        public string? VideoPath { get; set; }
        [Required]
        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
    }
}

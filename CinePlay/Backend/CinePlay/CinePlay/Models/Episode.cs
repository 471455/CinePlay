using System.ComponentModel.DataAnnotations;

namespace MyMDb.Models
{
    public class Episode : Media
    {
        [Required]
        public int SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
        [Required]
        public Guid SeriesId { get; set; }
        public virtual Series? Series { get; set; }
    }
}

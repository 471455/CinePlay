using MyMDb.Models.Base;

namespace MyMDb.DTOs
{
    public class SeriesDto : MediaDto
    {
        public int? Seasons { get; set; }
    }
}

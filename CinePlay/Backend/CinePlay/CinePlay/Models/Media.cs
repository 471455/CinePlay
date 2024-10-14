using Microsoft.AspNetCore.Mvc.ViewEngines;
using MyMDb.Models.Base;
using MyMDb.Data;

namespace MyMDb.Models
{
    public class Media : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? VideoPath { get; set; }
        public string? MediaType { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

        public Media()
        {
            Title = "N/A";
        }
    }
}

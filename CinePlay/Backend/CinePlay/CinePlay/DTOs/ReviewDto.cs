using MyMDb.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MyMDb.DTOs
{
    public class ReviewDto : BaseEntity
    {
        [Required]
        public Guid mediaId { get; set; }
        [Required]
        [Range(1.0, 10.0, ErrorMessage = "Derecelendirme 1 ile 10 arasında olmalıdır.")]
        public double? Rating { get; set; }
        public string? Comment { get; set; }
    }
}

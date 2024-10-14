using MyMDb.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MyMDb.Models
{
    public class UserProfile : BaseEntity
    {
        public string? ProfilePicPath { get; set; }
        public string? UserName { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}

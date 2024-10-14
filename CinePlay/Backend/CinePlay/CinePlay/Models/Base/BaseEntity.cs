using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyMDb.Models.Base
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        
        public virtual void UpdateDateModified()
        {
            DateModified = DateTime.Now;
        }
        public virtual void Initialize()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
    }
}

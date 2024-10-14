namespace MyMDb.Models
{
    public class Series : Media
    {
        public int? Seasons { get; set; }
        public virtual ICollection<Episode>? Episodes { get; set; }
    }
}

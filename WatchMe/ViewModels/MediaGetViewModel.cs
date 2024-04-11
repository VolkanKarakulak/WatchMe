using WatchMe.Models;

namespace WatchMe.ViewModels
{
    public class MediaGetViewModel
    {
        public string? MediaName { get; set; } 
        public string? Description { get; set; }
        public bool? IsPassive { get; set; }
        public float IMDBRating { get; set; }
        public List<string>? StarNames { get; set; }
        public List<string>? CategoryNames { get; set; }
        public List<string>? DirectorNames { get; set; }
        public List<string>? RestrictionNames { get; set; }

        

    }
}

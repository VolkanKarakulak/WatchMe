namespace WatchMe.ViewModels
{
    public class SearchMediaViewModel
    {
        public string? MediaName { get; set; }
        public string? Description { get; set; }
        public float IMDBRating { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Directors { get; set; }
        public List<string>? Restriction { get; set; }

    }
}

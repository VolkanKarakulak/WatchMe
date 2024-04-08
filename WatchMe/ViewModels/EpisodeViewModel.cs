namespace WatchMe.ViewModels
{
    public class EpisodeViewModel
    {
        public string? MediaName { get; set; }
        public byte SeasonNumber { get; set; }
        public short EpisodeNumber { get; set; }
        public string? Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
        public bool Passive { get; set; }
        public long ViewCount { get; set; }
    }
}

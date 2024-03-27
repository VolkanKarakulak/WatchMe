using System.ComponentModel.DataAnnotations.Schema;
using WatchMe.Identity.Data;

namespace WatchMe.Models
{
    public class UserWatched
    {
        public long UserId { get; set; }
        public long EpisodeId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }
        [ForeignKey("EpisodeId")]
        public Episode? Episode { get; set; }
    }
}

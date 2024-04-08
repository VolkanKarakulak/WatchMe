using System.ComponentModel.DataAnnotations.Schema;
using WatchMe.Identity.Data;

namespace WatchMe.Models
{
    public class UserFavorite
    {
        public long UserId { get; set; }
        public int MediaId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }
    }
}

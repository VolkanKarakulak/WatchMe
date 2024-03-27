using System.ComponentModel.DataAnnotations.Schema;

namespace WatchMe.Models
{
    public class MediaDirector
    {
        public int MediaId { get; set; }
        public int DirectorId { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }

        [ForeignKey("DirectorId")]
        public Director? Directors { get; set; }
    }
}

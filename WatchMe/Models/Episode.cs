using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchMe.Models
{
    public class Episode
    {
        public long Id { get; set; }
        public int MediaId { get; set; }

        [Range(0, byte.MaxValue)]
        public byte SeasonNumber { get; set; }
        [Range(0, 366)]
        [Column(TypeName = "nvarchar(500)")]
        public short EpisodeNumber { get; set; }

        [StringLength(200, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; } = "";
        public DateTime ReleaseDate { get; set; }

        [StringLength(500, MinimumLength = 1)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        public TimeSpan Duration { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }


    }
}

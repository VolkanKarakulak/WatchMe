using System.ComponentModel.DataAnnotations.Schema;

namespace WatchMe.Models
{
    public class MediaRestriction
    {
        public int MediaId { get; set; }
        public int RestrictionId { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }

        [ForeignKey("Restrictiond")]
        public Restriction? Restrictions { get; set; }

    }
}

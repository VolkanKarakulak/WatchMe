using System.ComponentModel.DataAnnotations.Schema;

namespace WatchMe.Models
{
    public class MediaCategory // cross table
    {

        public short CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public int MediaId { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }
    }
}

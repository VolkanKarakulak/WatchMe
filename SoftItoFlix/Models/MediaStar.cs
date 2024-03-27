using System.ComponentModel.DataAnnotations.Schema;

namespace SoftItoFlix.Models
{
    public class MediaStar
    {
        public int MediaId { get; set; }
        public int StarId { get; set; }

        [ForeignKey("MediaId")]
        public Media? Media { get; set; }

        [ForeignKey("StarId")]
        public Star? Star { get; set; }

    }
}

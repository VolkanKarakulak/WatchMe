using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WatchMe.Models
{
    public class Plan
    {
        public short Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = "";
        public float Price { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [StringLength(20, MinimumLength = 2)]
        public string Resolution { get; set; } = "";
    }
}

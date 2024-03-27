using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftItoFlix.Models
{
    public class Category
    {
        // byte yapacaktık ama ilerde sorun çıkarabilir diye short yaptık
        public short Id { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = "";
    }
}

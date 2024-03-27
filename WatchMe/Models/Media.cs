using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WatchMe.Models
{
    public class Media
    {
      
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = "";
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        public bool Passive { get; set; }
        public List<MediaCategory>? MediaCategories { get; set; }

        public List<MediaDirector>? MediaDirectors { get; set; }

        public List<MediaStar>? MediaStars { get; set; }

        public List<MediaRestriction>? MediaRestrictions { get; set; }

        [Range(0, 10)]
        public float IMDBRating { get; set; }



    }
}

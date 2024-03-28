using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WatchMe.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser<long>
{
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";

    public bool Passive { get; set; }

    [NotMapped]
    [StringLength(100, MinimumLength = 8)]
    public string PassWord { get; set; } = "";
}


using System.ComponentModel.DataAnnotations.Schema;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe.ViewModels
{
    public class UserPlanViewModel
    {
        public string? UserName { get; set; }

        public string? PlanName { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
      
        
    }
}

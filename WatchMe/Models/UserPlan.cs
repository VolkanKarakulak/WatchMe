using System.ComponentModel.DataAnnotations.Schema;
using WatchMe.Identity.Data;

namespace WatchMe.Models
{
    public class UserPlan
    {
        public long Id { get; set; }

        public long UserdId { get; set; }

        public short PlanId { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }

        [ForeignKey("PlanId")]
        public Plan? Plan { get; set; }
    }
}

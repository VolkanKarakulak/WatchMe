using Microsoft.AspNetCore.Identity;

namespace WatchMe.Models
{
    public class AppRole : IdentityRole<long>
    {
        public AppRole(string name) : base(name) { }
    }
}

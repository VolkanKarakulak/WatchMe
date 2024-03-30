using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe.Data
{
    public class WatchMeContext : IdentityDbContext<AppUser, AppUserRole, long>
    {
        public WatchMeContext(DbContextOptions<WatchMeContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MediaCategory>().HasKey(m=>new {m.MediaId, m.CategoryId});
            builder.Entity<MediaDirector>().HasKey(m=>new {m.MediaId, m.DirectorId});
            builder.Entity<MediaRestriction>().HasKey(m=>new {m.MediaId, m.RestrictionId});
            builder.Entity<MediaStar>().HasKey(m=>new {m.MediaId, m.StarId});
            builder.Entity<UserFavorite>().HasKey(m=>new {m.UserId, m.MediaId});
            builder.Entity<UserWatched>().HasKey(m=>new {m.UserId, m.EpisodeId});
            builder.Entity<Episode>().HasIndex(e => new { e.MediaId, e.SeasonNumber, e.EpisodeNumber }).IsUnique(true);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<WatchMe.Models.Category> Categories { get; set; } = default!;
        public DbSet<WatchMe.Models.Director> Directors { get; set; } = default!;
        public DbSet<WatchMe.Models.Episode> Episodes { get; set; } = default!;
        public DbSet<WatchMe.Models.Media> Medias { get; set; } = default!;
        public DbSet<WatchMe.Models.MediaCategory> MediaCategories { get; set; } = default!;
        public DbSet<WatchMe.Models.MediaDirector> MediaDirectors { get; set; } = default!;
        public DbSet<WatchMe.Models.MediaRestriction> MediaRestrictions { get; set; } = default!;
        public DbSet<WatchMe.Models.MediaStar> MediaStars { get; set; } = default!;
        public DbSet<WatchMe.Models.Plan> Plans { get; set; } = default!;
        public DbSet<WatchMe.Models.Restriction> Restrictions { get; set; } = default!;
        public DbSet<WatchMe.Models.Star> Stars { get; set; } = default!;
        public DbSet<WatchMe.Models.UserFavorite> UserFavorites { get; set; } = default!;
        public DbSet<WatchMe.Models.UserPlan> UserPlans { get; set; } = default!;
        public DbSet<WatchMe.Models.UserWatched> UserWatcheds { get; set; } = default!;





    }
}
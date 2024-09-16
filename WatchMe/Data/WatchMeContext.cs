using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe.Data
{
    public class WatchMeContext : IdentityDbContext<AppUser, AppRole, long>
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
        }

        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Director> Directors { get; set; } = default!;
        public DbSet<Episode> Episodes { get; set; } = default!;
        public DbSet<Media> Medias { get; set; } = default!;
        public DbSet<MediaCategory> MediaCategories { get; set; } = default!;
        public DbSet<MediaDirector> MediaDirectors { get; set; } = default!;
        public DbSet<MediaRestriction> MediaRestrictions { get; set; } = default!;
        public DbSet<MediaStar> MediaStars { get; set; } = default!;
        public DbSet<Plan> Plans { get; set; } = default!;
        public DbSet<Restriction> Restrictions { get; set; } = default!;
        public DbSet<Star> Stars { get; set; } = default!;
        public DbSet<UserFavorite> UserFavorites { get; set; } = default!;
        public DbSet<UserPlan> UserPlans { get; set; } = default!;
        public DbSet<UserWatched> UserWatcheds { get; set; } = default!;

    }
}
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Gig> Gigs { get; set; } //Refrence to Gig for EF. Gig already has reference to Genre class so EF will find it.
        public DbSet<Genre> Genres { get; set; } //But if we want to query list of Genre, we need dbset.
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>().
                HasRequired(a => a.Gig).
                WithMany(g => g.Attendances).
                WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(u => u.Followers).
                WithRequired(f => f.Followee).
                WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(u => u.Followees).
                WithRequired(f => f.Follower).
                WillCascadeOnDelete(false);

            modelBuilder.Entity<UserNotification>().
                HasRequired(n => n.User).
                WithMany(u => u.UserNotifications).
                WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
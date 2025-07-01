using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackRoom.DataAccess.Models;
using TrackRoom.Utilities;

namespace TrackRoom.DataAccess.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);

            builder.Entity<Member>()
                .HasOne(m => m.ApplicationUser)
                .WithMany()
                .HasForeignKey(m => m.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Member>()
                .HasOne(m => m.Meeting)
                .WithMany(m => m.Members)
                .HasForeignKey(m => m.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>()
                .HasData
                (
                     new IdentityRole() { Name = SD.Role_Host, ConcurrencyStamp = "1", NormalizedName = SD.Role_Host },
                     new IdentityRole() { Name = SD.Role_User, ConcurrencyStamp = "2", NormalizedName = SD.Role_User }
                );
        }
    }
}

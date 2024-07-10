using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection.Emit;

namespace SocialMedia.Models
{
    public class SocialContext : IdentityDbContext<User>
    {
        public SocialContext(DbContextOptions<SocialContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserUser> UserUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.ProfilePicture)
                .WithMany()
                .HasForeignKey(u => u.ProfilePictureId);

            // Configure the many-to-many relationship
            builder.Entity<UserUser>()
                .HasKey(uu => new { uu.UserId, uu.FriendId });

            builder.Entity<UserUser>()
                .HasOne(uu => uu.User)
                .WithMany(u => u.Following)
                .HasForeignKey(uu => uu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserUser>()
                .HasOne(uu => uu.Friend)
                .WithMany(u => u.Followers)
                .HasForeignKey(uu => uu.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasOne(p => p.Image)
                .WithMany()
                .HasForeignKey(p => p.ImageID);

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}

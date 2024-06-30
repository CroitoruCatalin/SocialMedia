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
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<UserUser> UserUsers { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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

            builder.Entity<User>()
                .HasOne(u => u.ProfilePicture)
                .WithOne(i => i.User)
                .HasForeignKey<Image>(i => i.UserId)
                .IsRequired(false); // If ProfilePictureId in User is nullable

            builder.Entity<Image>()
                .HasOne(i => i.User)
                .WithOne(u => u.ProfilePicture)
                .HasForeignKey<User>(u => u.ProfilePictureId)
                .IsRequired(false); // If UserId in Image is nullable

            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

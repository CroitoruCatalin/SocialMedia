using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}

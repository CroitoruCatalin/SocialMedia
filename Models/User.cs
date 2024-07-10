using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = "USERNAME_NOT_FOUND";
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public int ProfilePictureId { get; set; } = new int();
        public virtual Image ProfilePicture { get; set; } = new Image
        {
            FileName = "default.png",
            ContentType = "image/png",
            Data = File.ReadAllBytes("wwwroot/assets/images/default.png"),
            UploadDate = DateTime.UtcNow
        };
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<UserUser> Followers { get; set; } = new List<UserUser>();
        public virtual ICollection<UserUser> Following { get; set; } = new List<UserUser>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

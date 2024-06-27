using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? ProfilePictureId {  get; set; }
        public virtual Image? ProfilePicture { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public virtual ICollection<UserUser> Followers { get; set; } = new List<UserUser>();
        public virtual ICollection<UserUser> Following { get; set; } = new List<UserUser>();
    }
}

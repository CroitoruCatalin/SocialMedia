using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Post
    {
        public int PostID { get; set; }
        [MinLength(10, ErrorMessage = "Posts must contain at least 10 characters.")]
        [StringLength(280, ErrorMessage = "Posts can only contain up to 280 characters.")]
        public string Content { get; set; }
        public string? UserID { get; set; }
        public User? User { get; set; }

        public DateTime? CreationDate { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostLike>? Likes { get; set; }
    }
}

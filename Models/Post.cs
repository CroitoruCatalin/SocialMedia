using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Post : ContentBase
    {
        [MinLength(10, ErrorMessage = "Posts must contain at least 10 characters.")]
        [StringLength(280, ErrorMessage = "Posts can only contain up to 280 characters.")]
        public override string Message { get; set; } = string.Empty;
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public int CommentCount { get; set; } = 0;
    }
}

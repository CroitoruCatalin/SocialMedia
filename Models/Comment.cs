using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Comment : ContentBase 
    {
        [MinLength(5, ErrorMessage = "Comments must contain at least 5 characters.")]
        [StringLength(140, ErrorMessage = "Comments can only contain up to 140 characters.")]
        public override string Message { get; set; } = string.Empty;

        //parent post
        public int PostID { get; set; } = new int();
        public Post Post { get; set; } = new Post();
    }
}

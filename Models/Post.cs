namespace SocialMedia.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string? Content { get; set; }
        public string? UserID { get; set; }
        public User? User { get; set; }

        public DateTime? CreationDate { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostLike>? Likes { get; set; }
    }
}

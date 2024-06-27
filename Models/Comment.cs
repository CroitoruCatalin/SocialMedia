namespace SocialMedia.Models
{
    public class Comment
    {
        public int CommentID {  get; set; }
        public string? Message { get; set; }
        public string? UserID {get; set; }
        public User? User { get; set; }
        public int PostID { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CommentLike>? Likes { get; set; }
    }
}

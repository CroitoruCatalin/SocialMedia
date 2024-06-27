namespace SocialMedia.Models
{
    public class CommentLike
    {
        public int CommentLikeID { get; set; }
        public int CommentID { get; set; }
        public string UserID { get; set; }
        public bool LikeValue { get; set; }
    }
}

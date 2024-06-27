namespace SocialMedia.Models
{
    public class PostLike
    {
        public int PostLikeID { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public bool LikeValue { get; set; }
    }
}

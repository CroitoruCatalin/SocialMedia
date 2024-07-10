using System.Security.Cryptography.X509Certificates;

namespace SocialMedia.Models
{
    public class Reaction
    {
        public int ReactionID { get; set; }
        public short Value { get; set; } = 0;

        public string UserID { get; set; } = String.Empty;
        public User User { get; set; } = new User();

        public int ReactableID { get; set; } = new int();
        public ReactableType ReactableType { get; set; } = new ReactableType();
    }

    public enum ReactableType
    {
        Post,
        Comment
    }
}


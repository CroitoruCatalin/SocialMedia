using System.Security.Cryptography.X509Certificates;

namespace SocialMedia.Models
{
    public class Reaction
    {
        public int ReactionID { get; set; }
        public short Value { get; set; } = 0;

        public string UserID {  get; set; }
        public User User { get; set; }

        public int ReactableID {  get; set; }
        public ReactableType ReactableType {  get; set; }
    }

    public enum ReactableType
    {
        Post,
        Comment
    }
}



using SocialMedia.Controllers;

namespace SocialMedia.Models
{
    public abstract class ContentBase
    {
        public int ID { get; set; }

        //actual content
        public virtual string Message { get; set; }

        public Image? Image { get; set; }

        //author
        public string? UserID { get; set; }
        public User? User {  get; set; }


        //dates
        public DateTime CreationDate {  get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set;} = DateTime.UtcNow;

        //likes/reactions
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public int LikeCount { get; set; } = 0;
        public int DislikeCount {  get; set; } = 0;

    }
}

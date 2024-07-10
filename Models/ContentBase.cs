
using SocialMedia.Controllers;

namespace SocialMedia.Models
{
    public abstract class ContentBase
    {
        public int ID { get; set; }

        //actual content
        public virtual string Message { get; set; } = String.Empty;

        public int? ImageID { get; set; }
        public Image? Image { get; set; }

        public virtual string Embed { get; set; } = String.Empty;

        //author
        public string UserID { get; set; } = String.Empty;
        public User User { get; set; } = new User();


        //dates
        public DateTime CreationDate {  get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set;} = DateTime.UtcNow;

        //likes/reactions
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public int LikeCount { get; set; } = 0;
        public int DislikeCount {  get; set; } = 0;



    }
}

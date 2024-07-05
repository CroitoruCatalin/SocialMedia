using Microsoft.Data.SqlClient;

namespace SocialMedia.Models
{

    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType Type {  get; set; }

        //can indicate the id of a post, of a comment, or of a user
        public int SourceID { get; set; }
        //the person who created the notification
        public string InstigatorID { get; set; }


        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public string Url { get; set; }

    }
    public enum NotificationType
    {
        NewComment,
        NewFollower,
    }
}

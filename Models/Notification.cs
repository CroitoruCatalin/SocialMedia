using Microsoft.Data.SqlClient;

namespace SocialMedia.Models
{

    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; } = String.Empty;
        public User User { get; set; } = new User();
        public string Message { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; } = new NotificationType();

        //can indicate the id of a post, of a comment, or of a user
        public int SourceID { get; set; } = new int();
        //the person who created the notification
        public string InstigatorID { get; set; } = String.Empty;


        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public string Url { get; set; } = String.Empty;

    }
    public enum NotificationType
    {
        NewComment,
        NewFollower,
    }
}

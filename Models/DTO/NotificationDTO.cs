namespace SocialMedia.Models.DTO
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserProfilePictureData { get; set; } // Base64 encoded image data
        public string UserProfilePictureContentType { get; set; } // Content type of the image
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }
        public int SourceID { get; set; }
        public string InstigatorID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Url { get; set; }
        public string FullName { get; set; }

        public NotificationDTO(Notification notification)
        {
            NotificationID = notification.NotificationID;
            UserID = notification.UserID;
            UserName = notification.UserName;
            UserProfilePictureData = ""; // Default empty; set in service
            UserProfilePictureContentType = ""; // Default empty; set in service
            Message = notification.Message;
            IsRead = notification.IsRead;
            Type = notification.Type;
            SourceID = notification.SourceID;
            InstigatorID = notification.InstigatorID;
            CreationDate = notification.CreationDate;
            Url = notification.Url;
            FullName = notification.UserName;
        }
    }

}

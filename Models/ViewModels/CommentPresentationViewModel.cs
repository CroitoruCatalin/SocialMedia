namespace SocialMedia.Models.ViewModels
{
    public class CommentPresentationViewModel
    {
        public int ID { get; set; } = new int();
        public string Message { get; set; } = "DEFAULT_MESSAGE";
        public string UserID { get; set; } = String.Empty;
        public UserPresentationViewModel User { get; set; } = new UserPresentationViewModel();
        public DateTime CreationDate {  get; set; } = DateTime.MinValue;
        public DateTime ModifiedDate { get; set; } = DateTime.MinValue;
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        public CommentPresentationViewModel(Comment comment)
        {
            ID = comment.ID;
            Message = comment.Message;
            UserID = comment.UserID;
            User = new UserPresentationViewModel(comment.User);
            CreationDate = comment.CreationDate;
            ModifiedDate = comment.ModifiedDate;
            LikeCount = comment.LikeCount;
            DislikeCount = comment.DislikeCount;
            Reactions = comment.Reactions;
        }

        public CommentPresentationViewModel()
        {
            ID = new int();
            Message = "DEFAULT_MESSAGE";
            UserID = String.Empty;
            User = new UserPresentationViewModel();
            CreationDate = DateTime.MinValue;
            ModifiedDate = DateTime.MinValue;
            LikeCount = 0;
            DislikeCount = 0;
            Reactions = new List<Reaction>();
        }
    }
}

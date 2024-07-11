namespace SocialMedia.Models.ViewModels
{
    public class PostPresentationViewModel
    {
        //post id
        public int ID { get; set; } = new int();

        //author
        public string UserID { get; set; } = String.Empty;
        public UserPresentationViewModel User { get; set; } = new UserPresentationViewModel();

        public DateTime CreationDate { get; set; } = DateTime.MinValue;
        public DateTime ModifiedDate { get; set; } = DateTime.MinValue;

        public string Message { get; set; } = String.Empty;
        public int? ImageID { get; set; }
        public ImageViewModel? Image { get; set; }
        public string YoutubeLink { get; set; } = String.Empty;
        public string YoutubeEmbed { get; set; } = String.Empty;


        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();


        public int CommentCount { get; set; } = new int();
        public CommentPresentationViewModel TopComment { get; set; } = new CommentPresentationViewModel();

        public PostPresentationViewModel(Post post)
        {
            ID = post.ID;
            UserID = post.UserID;
            CreationDate = post.CreationDate;
            ModifiedDate = post.ModifiedDate;
            Message = post.Message;
            ImageID = post.ImageID is null ? null : post.ImageID; 
            Image = post.Image is null ? null : new ImageViewModel(post.Image);
            LikeCount = post.LikeCount;
            DislikeCount = post.DislikeCount;
            Reactions = post.Reactions;
            CommentCount = post.CommentCount;
            User = new UserPresentationViewModel(post.User);
            TopComment = new CommentPresentationViewModel(post.Comments.OrderByDescending(c => c.CreationDate).FirstOrDefault()?? new Comment());
            YoutubeEmbed = post.Embed;
        }

        public PostPresentationViewModel()
        {
            ID = -1;
            UserID = String.Empty;
            User = new UserPresentationViewModel();
            CreationDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
            Message = String.Empty;
            ImageID = -1;
            Image = new ImageViewModel();
            YoutubeLink = String.Empty;
            YoutubeEmbed = String.Empty;
            LikeCount = 0;
            DislikeCount = 0;
            Reactions = new List<Reaction>();
            CommentCount = 0;
            TopComment = new CommentPresentationViewModel();
        }


        public string GetDetails(string caller)
        {
            var userDetails = User?.GetDetails(caller) ?? "No User Details";
            var imageDetails = Image?.GetDetails(caller) ?? "No Image Details";

            return $"PostPresentationViewModel inside {caller} contains:" +
                   $"\nID={ID}" +
                   $"\nUserID={UserID}" +
                   $"\nCreationDate={CreationDate}" +
                   $"\nModifiedDate={ModifiedDate}" +
                   $"\nMessage={Message}" +
                   $"\nImageId={ImageID}" +
                   $"\nYoutubeLink={YoutubeLink}" +
                   $"\nYoutubeEmbed={YoutubeEmbed}" +
                   $"\nReactions={Reactions?.Count ?? 0}" +
                   $"\nWith the following User: {userDetails}" +
                   $"\nWith the following Image: {imageDetails}";
        }
    }
}

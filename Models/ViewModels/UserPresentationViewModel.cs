namespace SocialMedia.Models.ViewModels
{
    public class UserPresentationViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public int? ProfilePictureId { get; set; }
        public ImageViewModel ProfilePicture { get; set; }

        public UserPresentationViewModel(User user)
        {
            Id = user.Id;
            FullName = user.FullName;
            ProfilePictureId = user.ProfilePictureId;
            ProfilePicture = new ImageViewModel(user.ProfilePicture);
        }
        public UserPresentationViewModel()
        {
            Id = String.Empty;
            FullName = String.Empty;
            ProfilePictureId = new int();
            ProfilePicture = new ImageViewModel();
        }

        public string GetDetails(string caller)
        {
            return "UserPresentationViewModel in "+caller+"has:\nId="+Id+"\nFullName="+FullName+"\nProfilePictureId="+ProfilePictureId
                +" with the following profile picture:"+ProfilePicture.GetDetails(caller);
        }
    }
}

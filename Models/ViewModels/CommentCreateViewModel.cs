using SocialMedia.Models;
namespace SocialMedia.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Comment Comment {  get; set; }
        public Post Post { get; set; }
    }
}

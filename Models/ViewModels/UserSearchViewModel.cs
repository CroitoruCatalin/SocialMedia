namespace SocialMedia.Models.ViewModels
{
    public class UserSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<UserSearchResultViewModel> Results { get; set; }
    }
}

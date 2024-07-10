using SocialMedia.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia.Repositories.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<Post?> GetPostByIdAsync(int postId);

        Task<PostPresentationViewModel> GetPostPresentationWithTopCommentAsync(int postId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);

        Task<IEnumerable<int>> GetUserPostsAsync(string userId);
    }
}

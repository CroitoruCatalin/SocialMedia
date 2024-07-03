using SocialMedia.Models;

namespace SocialMedia.Repositories.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<Post> GetPostByIdAsync(int postId);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);
    }
}

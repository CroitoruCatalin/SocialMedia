using SocialMedia.Models;
using System.Security.Claims;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post> GetPostByIdAsync(int postId);
        List<Post> GetAllPosts();
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task CreatePostAsync(Post post, ClaimsPrincipal userPrincipal);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
        public Post GetPostWithComments(int postId);
    }
}

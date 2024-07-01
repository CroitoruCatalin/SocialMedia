using SocialMedia.Models;
using System.Security.Claims;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostService
    {
        Task<Post?> GetPostById(int postId);
        List<Post> GetAllPosts();
        Task<List<Post>> GetAllPostsAsync();
        Task CreatePost(Post post, ClaimsPrincipal userPrincipal);
        Task UpdatePost(Post post);
        Task DeletePost(int postId);
        public Post GetPostWithComments(int postId);
    }
}

using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
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
        public Post GetPostWithCommentsAsync(int postId);

        Task<IEnumerable<int>> GetPostRecommendationsAsync(string userId);
        Task<IEnumerable<int>> RecommendationAlgorithm(string userId);
    }
}

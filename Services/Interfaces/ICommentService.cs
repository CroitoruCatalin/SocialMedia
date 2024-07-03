using SocialMedia.Models;

namespace SocialMedia.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task<bool> CommentExists(int commentId);
    }
}

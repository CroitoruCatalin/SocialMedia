using SocialMedia.Models;

namespace SocialMedia.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentById(int commentId);
        Task<IEnumerable<Comment>> GetAllComments();
        Task CreateComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(int commentId);
        Task<bool> CommentExists(int commentId);
    }
}

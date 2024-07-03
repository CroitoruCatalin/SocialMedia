using SocialMedia.Models;

namespace SocialMedia.Repositories.Interfaces
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
    }
}

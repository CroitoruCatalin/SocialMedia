using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            var comment = await _SocialContext.Comments
                .Include(c => c.Reactions)
                .FirstOrDefaultAsync(c => c.ID == commentId);

            if(comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _SocialContext.Comments
                .Where(c => c.PostID == postId)
                .Include(c => c.Reactions)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _SocialContext.Comments.AddAsync(comment);
            await _SocialContext.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _SocialContext.Comments.Update(comment);
            await _SocialContext.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _SocialContext.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _SocialContext.Comments.Remove(comment);
                await _SocialContext.SaveChangesAsync();
            }
        }

    }
}

using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CommentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == commentId)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _repositoryWrapper.CommentRepository
                .FindAll()
                .ToListAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            _repositoryWrapper.CommentRepository.Create(comment);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _repositoryWrapper.CommentRepository.Update(comment);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == commentId)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                _repositoryWrapper.CommentRepository.Delete(comment);
                await _repositoryWrapper.SaveAsync();
            }
        }
        public async Task<bool> CommentExists(int id)
        {
            return await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == id).AnyAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _repositoryWrapper.CommentRepository
                .GetCommentsByPostIdAsync(postId);
        }
    }
}

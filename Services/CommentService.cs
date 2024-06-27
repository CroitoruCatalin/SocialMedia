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

        public async Task<Comment?> GetCommentById(int commentId)
        {
            return await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.CommentID == commentId)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _repositoryWrapper.CommentRepository
                .FindAll()
                .ToListAsync();
        }

        public async Task CreateComment(Comment comment)
        {
            _repositoryWrapper.CommentRepository.Create(comment);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateComment(Comment comment)
        {
            _repositoryWrapper.CommentRepository.Update(comment);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.CommentID == commentId)
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
                .FindByCondition(c => c.CommentID == id).AnyAsync();
        }
    }
}

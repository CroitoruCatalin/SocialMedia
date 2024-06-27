using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class CommentLikeService : ICommentLikeService
    {
        private IRepositoryWrapper _repositoryWrapper;
        public CommentLikeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task DislikeComment(int commentId, string userId)
        {
            var existingLike = _repositoryWrapper
                .CommentLikeRepository
                .FindByCondition(pl => pl.CommentID == commentId && pl.UserID == userId)
                .FirstOrDefault();

            if (existingLike != null)
            {
                if (existingLike.LikeValue)
                {
                    existingLike.LikeValue = false;
                    _repositoryWrapper.CommentLikeRepository.Update(existingLike);
                }
            }
            else
            {
                var newLike = new CommentLike
                {
                    CommentID = commentId,
                    UserID = userId,
                    LikeValue = false
                };
                _repositoryWrapper.CommentLikeRepository.Create(newLike);
            }
            await _repositoryWrapper.SaveAsync();
        }

        public async Task LikeComment(int commentId, string userId)
        {
            var existingLike = _repositoryWrapper
                .CommentLikeRepository
                .FindByCondition(pl => pl.CommentID == commentId && pl.UserID == userId)
                .FirstOrDefault();

            if (existingLike != null)
            {
                if (!existingLike.LikeValue)
                {
                    existingLike.LikeValue = true;
                    _repositoryWrapper.CommentLikeRepository.Update(existingLike);
                }
            }
            else
            {
                var newLike = new CommentLike
                {
                    CommentID = commentId,
                    UserID = userId,
                    LikeValue = true
                };
                _repositoryWrapper.CommentLikeRepository.Create(newLike);

            }
            await _repositoryWrapper.SaveAsync();
        }

    }
}

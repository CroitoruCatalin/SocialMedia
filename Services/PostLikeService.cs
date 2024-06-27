using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class PostLikeService : IPostLikeService
    {
        private IRepositoryWrapper _repositoryWrapper;
        public PostLikeService(IRepositoryWrapper repositoryWrapper) {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task DislikePost(int postId, string userId)
        {
            var existingLike = _repositoryWrapper
                .PostLikeRepository
                .FindByCondition(pl => pl.PostID == postId && pl.UserID == userId)
                .FirstOrDefault();

            if (existingLike != null)
            {
                if (existingLike.LikeValue)
                {
                    existingLike.LikeValue = false;
                    _repositoryWrapper.PostLikeRepository.Update(existingLike);
                }
            }
            else
            {
                var newLike = new PostLike
                {
                    PostID = postId,
                    UserID = userId,
                    LikeValue = false
                };
                _repositoryWrapper.PostLikeRepository.Create(newLike);
            }
            await _repositoryWrapper.SaveAsync();
        }

        public async Task LikePost(int postId, string userId)
        {
            var existingLike = _repositoryWrapper
                .PostLikeRepository
                .FindByCondition(pl => pl.PostID == postId && pl.UserID == userId)
                .FirstOrDefault();

            if (existingLike != null)
            {
                if (!existingLike.LikeValue)
                {
                    existingLike.LikeValue = true;
                    _repositoryWrapper.PostLikeRepository.Update(existingLike);
                }
            }
            else
            {
                var newLike = new PostLike
                {
                    PostID = postId,
                    UserID = userId,
                    LikeValue = true
                };
                _repositoryWrapper.PostLikeRepository.Create(newLike);
             
            }
            await _repositoryWrapper.SaveAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class ReactionService : IReactionService
    {
        private readonly SocialContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;
        public ReactionService(
            SocialContext context,
            IRepositoryWrapper repositoryWrapper, 
            UserManager<User> userManager,
            IPostService postService)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _postService = postService;
        }

        public async Task AddOrUpdateReactionAsync(Reaction reaction, int postId)
        {
            var existingReaction = await GetUserReactionAsync(reaction.UserID, reaction.ReactableID);
            var post = await _postService.GetPostByIdAsync(postId);

            if (existingReaction != null)
            {
                post.Reactions.Remove(existingReaction);

                existingReaction.Value = reaction.Value;

                post.Reactions.Add(existingReaction);

                _context.Reactions.Update(existingReaction);
            }
            else
            {
                post.Reactions.Add(reaction);
                await CreateReactionAsync(reaction);
            }

            //determine new counts
            post.LikeCount = post.Reactions.Count(r => r.Value == 1);
            post.DislikeCount = post.Reactions.Count(r => r.Value == -1);

            await _postService.UpdatePostAsync(post);

            await _context.SaveChangesAsync();
        }

        public Task CreateReactionAsync(Reaction reaction)
        {
            return _repositoryWrapper.ReactionRepository
                .AddReactionAsync(reaction);
        }

        public Task DeleteReactionAsync(int reactionId)
        {
            return _repositoryWrapper.ReactionRepository
                .RemoveReactionAsync(reactionId);
        }

        public async Task<Reaction> GetReactionByIdAsync(int reactionId)
        {
            return await _repositoryWrapper.ReactionRepository
                .GetReactionByIdAsync(reactionId);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsByEntityAsync(int entityId, ReactableType type)
        {
            return await _repositoryWrapper.ReactionRepository
                .GetReactionsByEntityAsync (entityId, type);
        }

        public async Task<Reaction> GetUserReactionAsync(string userId, int postId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(
                r => r.UserID == userId && 
                r.ReactableID == postId && 
                r.ReactableType == ReactableType.Post);
        }
    }
}

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
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;
        public ReactionService(
            IRepositoryWrapper repositoryWrapper, 
            UserManager<User> userManager,
            IPostService postService)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _postService = postService;
        }

        public async Task AddOrUpdateReactionAsync(Reaction reaction, Post post)
        {
            var existingReaction = await GetUserReactionAsync(reaction.UserID, reaction.ReactableID);

            

            //if the user has reacted, switch reaction value
            if (existingReaction != null && post != null)
            {

                if (existingReaction.Value == reaction.Value)
                {
                    return;
                }

                post.Reactions.Remove(existingReaction);

                Console.WriteLine("REACTION SERVICE Reaction already exists with value: " + existingReaction.Value +" for post id=" + post.ID + " with " + post.LikeCount 
                    + " likes and " + post.DislikeCount + " dislikes!");
                //multiple value by -1 to get the oposite
                existingReaction.Value = reaction.Value;
                //switch the like and dislike count of the post based on the current value
                post.LikeCount += existingReaction.Value > 0 ? 1 : -1;
                post.DislikeCount += existingReaction.Value < 0 ? 1 : -1;

                Console.WriteLine("REACTION SERVICE Reaction new value: " + existingReaction.Value);
                post.Reactions.Add(existingReaction);
                _repositoryWrapper.ReactionRepository.UpdateReaction(existingReaction);
            }
            else if(post!= null)
            {
                post.Reactions.Add(reaction);

                post.LikeCount += reaction.Value > 0 ? 1 : 0;
                post.DislikeCount += reaction.Value < 0 ? 1 : 0;

                await _repositoryWrapper.ReactionRepository.AddReactionAsync(reaction);

                Console.WriteLine("REACTION SERVICE Adding New Reacion with value: " + reaction.Value + " for post id=" + post.ID + " with " + post.LikeCount
                    + " likes and " + post.DislikeCount + " dislikes!");
            }

            await _repositoryWrapper.PostRepository.UpdatePostAsync(post);
            await _repositoryWrapper.SaveAsync();
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

        public async Task<Reaction?> GetUserReactionAsync(string userId, int postId)
        {
            return await _repositoryWrapper.ReactionRepository
                .GetUserReactionToPostAsync(userId, postId);
        }
    }
}

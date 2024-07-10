using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(SocialContext socialContext)
            : base(socialContext) { }

        public async Task<Reaction?> GetReactionByIdAsync(int reactionId)
        {
            return await _SocialContext.Reactions
                .FirstOrDefaultAsync(r => r.ReactionID == reactionId);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsByEntityAsync(int entityId, ReactableType type)
        {
            return await _SocialContext.Reactions
                .Where(r => r.ReactableID == entityId && r.ReactableType == type)
                .ToListAsync();
        }

        public async Task AddReactionAsync(Reaction reaction)
        {
            await _SocialContext.Reactions.AddAsync(reaction);
        }

        public async Task RemoveReactionAsync(int reactionId)
        {
            var reaction = await _SocialContext.Reactions.FindAsync(reactionId);
            if (reaction != null)
            {
                _SocialContext.Reactions.Remove(reaction);
            }
        }

        public async Task<Reaction?> GetUserReactionToPostAsync(string userId, int postId)
        {
            return await _SocialContext.Reactions
                .FirstOrDefaultAsync(
                r => r.UserID == userId &&
                r.ReactableID == postId &&
                r.ReactableType == ReactableType.Post);
        }

        public Reaction UpdateReaction(Reaction reaction)
        {
            _SocialContext.Reactions.Update(reaction);
            return reaction;
        }
    }
}

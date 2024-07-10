using SocialMedia.Models;

namespace SocialMedia.Repositories.Interfaces
{
    public interface IReactionRepository
    {
        Task<Reaction?> GetReactionByIdAsync(int reactionId);
        Task<IEnumerable<Reaction>> GetReactionsByEntityAsync(int entityId, ReactableType type);
        Task AddReactionAsync(Reaction reaction);
        Task RemoveReactionAsync(int reactionId);
        Reaction UpdateReaction(Reaction reaction);
        Task<Reaction?> GetUserReactionToPostAsync(string userId, int postId);
    }
}

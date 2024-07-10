using SocialMedia.Models;
namespace SocialMedia.Services.Interfaces
{
    public interface IReactionService
    {
        Task<Reaction> GetReactionByIdAsync(int reactionId);
        Task<IEnumerable<Reaction>> GetReactionsByEntityAsync(int entityId, ReactableType type);
        Task CreateReactionAsync(Reaction reaction);
        Task DeleteReactionAsync(int reactionId);

        Task<Reaction?> GetUserReactionAsync(string userId, int postId);
        Task AddOrUpdateReactionAsync(Reaction reaction, Post post);
    }
}

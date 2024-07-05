using SocialMedia.Models;

namespace SocialMedia.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {

        Task<User> GetUserByIdAsync(string Id);
        Task FollowUser(UserUser userUser);
        Task UnfollowUser(UserUser userUser);
        Task<List<User>> GetFollowers(string userId);
        Task<List<User>> SearchUsersAsync(string searchTerm, int maxResults = 10);
    }
}

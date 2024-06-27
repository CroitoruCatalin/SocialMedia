using SocialMedia.Models;

namespace SocialMedia.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task FollowUser(UserUser userUser);
        Task UnfollowUser(UserUser userUser);
        Task<List<User>> GetFollowers(string userId);
    }
}

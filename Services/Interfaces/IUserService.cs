using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Models.ViewModels;

namespace SocialMedia.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(string userId);
        Task<User?> GetUserByIdWithPosts(string userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(string userId);
        Task<bool> UserExists(string userId);

        Task<int> FollowUser(string userId, string followUserId);
        Task<int> UnfollowUser(string userId, string followUserId);
        Task<IEnumerable<UserViewModel>> GetFollowers(string userId);
        Task<bool> IsFollowing(string userId, string followUserId);
        Task<User?> GetUserByIdWithPostsAndFollowers(string userId);

        Task<List<User>> SearchUsersAsync(string searchTerm, int maxResults = 10);

        UserPresentationViewModel GetUserPresentation(string userId);
    }
}

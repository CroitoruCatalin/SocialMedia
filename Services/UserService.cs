﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public UserService(IRepositoryWrapper repositoryWrapper, 
            UserManager<User> userManager,
            INotificationService notificationService
            )
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<User?> GetUserById(string userId)
        {
            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdWithPosts(string userId)
        {
            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Reactions)
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .ThenInclude(c=>c.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repositoryWrapper.UserRepository
                .FindAll()
                .ToListAsync();
        }

        public async Task CreateUser(User user)
        {
            _repositoryWrapper.UserRepository.Create(user);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateUser(User user)
        {
            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                _repositoryWrapper.UserRepository.Delete(user);
                await _repositoryWrapper.SaveAsync();
            }
        }
        public async Task<bool> UserExists(string id)
        {
            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == id).AnyAsync();
        }

        public async Task<int> FollowUser(string userId, string followUserId)
        {
            var userUser = new UserUser { UserId = userId, FriendId = followUserId };
            await _repositoryWrapper.UserRepository.FollowUser(userUser);
            await _repositoryWrapper.SaveAsync();



            //notify the user who is being followed
            var followee = await _userManager.FindByIdAsync(followUserId);
            if(followee != null)
            {
                var follower = await _userManager.FindByIdAsync(userId);
                if(follower != null)
                {

                    Notification notification = new Notification
                    {
                        UserID = followee.Id,
                        User = followee,
                        Message = $"{follower.FullName} is now following you!",
                        Type = NotificationType.NewFollower,
                        InstigatorID = follower.Id,
                        Url = $"/Users/Details/{follower.Id}"
                };
                    _repositoryWrapper.NotificationRepository.Create(notification);
                    followee.Notifications.Add(notification);
                    _repositoryWrapper.UserRepository.Update(followee);

                    var message = notification.Message;
                    var url = notification.Url;
                    await _notificationService
                        .SendNotificationAsync(followee.Id, notification);

                }
            }



            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == followUserId)
                .Select(u => u.Followers.Count)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UnfollowUser(string userId, string followUserId)
        {
            var userUser = new UserUser { UserId = userId, FriendId = followUserId };
            await _repositoryWrapper.UserRepository.UnfollowUser(userUser);
            await _repositoryWrapper.SaveAsync();

            // Return the updated followers count
            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == followUserId)
                .Select(u => u.Followers.Count)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserViewModel>> GetFollowers(string userId)
        {
            var user = await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .Include(u => u.Followers)
                .ThenInclude(f => f.User)  // Include the User navigation property
                .FirstOrDefaultAsync();

            if (user == null || user.Followers == null)
            {
                return Enumerable.Empty<UserViewModel>();
            }

            var followers = user.Followers.Select(f => new UserViewModel
            {
                Id = f.User.Id,
                FullName = f.User.FullName
            }).ToList();

            return followers;
        }

        public async Task<bool> IsFollowing(string userId, string followUserId)
        {
            var user = await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .Include(u => u.Following)
                .FirstOrDefaultAsync();

            if (user == null || user.Following == null)
            {
                return false;
            }

            return user.Following.Any(f => f.FriendId == followUserId);
        }
        public async Task<User?> GetUserByIdWithPostsAndFollowers(string userId)
        {
            return await _repositoryWrapper.UserRepository
                .FindByCondition(u => u.Id == userId)
                .Include(u => u.ProfilePicture)
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Reactions)
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.User)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm, int maxResults = 10)
        {
            return await _repositoryWrapper
                .UserRepository.SearchUsersAsync(searchTerm, maxResults);
        }

        public UserPresentationViewModel GetUserPresentation(string userId)
        {
            return _repositoryWrapper.UserRepository.GetUserPresentation(userId);
        }
    }
}

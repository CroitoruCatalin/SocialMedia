using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly SocialContext _context;
        public UserRepository(SocialContext socialContext)
            : base(socialContext)
        {
            _context = socialContext;
        }
        public async Task FollowUser(UserUser userUser)
        {
            await _context.UserUsers.AddAsync(userUser);
        }

        public async Task UnfollowUser(UserUser userUser)
        {
            var userUserEntity = await _context.UserUsers
                .FirstOrDefaultAsync(uu => uu.UserId == userUser.UserId && uu.FriendId == userUser.FriendId);
            if (userUserEntity != null)
            {
                _context.UserUsers.Remove(userUserEntity);
            }
        }

        public async Task<List<User>> GetFollowers(string userId)
        {
            return await _context.UserUsers
                .Where(uu => uu.FriendId == userId)
                .Select(uu => uu.User)
                .ToListAsync();
        }

    }
}

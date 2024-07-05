using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(SocialContext socialContext)
            : base(socialContext)
        { 
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _SocialContext.Notifications
                .AddAsync(notification);
            await _SocialContext.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            var _notification = await _SocialContext.Notifications
                .FindAsync(notification.NotificationID);
            if(_notification != null)
            {
                _SocialContext.Notifications.Remove(notification);
                await _SocialContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _SocialContext.Notifications
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            return await _SocialContext.Notifications
                .FirstOrDefaultAsync(n => n.NotificationID == notificationId);
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _SocialContext.Notifications.Update(notification);
            await _SocialContext.SaveChangesAsync();
        }
    }
}

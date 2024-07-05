using SocialMedia.Models;

namespace SocialMedia.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsForUserAsync(string userId);
        Task SendNotificationAsync(string userId, Notification notification);
    }
}

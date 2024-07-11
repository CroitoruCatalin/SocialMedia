using SocialMedia.Models;
using SocialMedia.Models.DTO;

namespace SocialMedia.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsForUserAsync(string userId);
        Task SendNotificationAsync(string userId, Notification notification);
        Task<NotificationDTO?> GetNotificationDTO(string userId, Notification notification);
    }
}

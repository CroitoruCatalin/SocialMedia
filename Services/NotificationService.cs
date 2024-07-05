using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Hubs;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly SocialContext _context;
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly UserManager<User> _userManager;
        private readonly UserConnectionManager _connectionManager;

        public NotificationService(IRepositoryWrapper repositoryWrapper, 
            SocialContext context,
            IHubContext<NotificationsHub> hubContext,
            UserManager<User> userManager,
            UserConnectionManager connectionManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
            _connectionManager = connectionManager;
        }



        public async Task SendNotificationAsync(string userId, Notification notification)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string senderId = notification.InstigatorID;
            var instigator = await _userManager.FindByIdAsync(senderId);
            Image image = await _context.Images.FirstOrDefaultAsync(i => i.ImageId == instigator.ProfilePictureId);

            Console.WriteLine($"Found profile picture with id {image.ImageId}, of user {image.UserId} with content type {image.ContentType} with data {image.Data}");

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            if(user != null)
            {
                var profilePictureData = image != null
                    ? Convert.ToBase64String(image.Data)
                    : string.Empty;


                NotificationDTO notificationDTO = new NotificationDTO(notification);
                notificationDTO.UserProfilePictureData = profilePictureData;
                notificationDTO.UserProfilePictureContentType = image.ContentType ?? "image/png";

                Console.WriteLine($"Converted profile picture to data {notificationDTO.UserProfilePictureData[1]}, and content type {notificationDTO.UserProfilePictureContentType}");

                var connectionId = _connectionManager.GetConnectionId(userId);
                if (connectionId != null)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", notificationDTO);
                }
            }
        }

        public async Task<List<Notification>> GetNotificationsForUserAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.CreationDate)
                .ToListAsync();
        }

    }
}

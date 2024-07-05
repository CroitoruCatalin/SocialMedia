using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
namespace SocialMedia.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        private readonly UserConnectionManager _connectionManager;
        private readonly UserManager<User> _userManager;
        public NotificationsHub(
            UserConnectionManager connectionManager, UserManager<User> userManager)
        {
            _connectionManager = connectionManager;
            _userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            _connectionManager.AddConnection(userId, Context.ConnectionId);

            Console.WriteLine($"OnConnectedAsync: Context.Useridentifier[{userId}] and Context.ConnectionId[{Context.ConnectionId}]");

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            _connectionManager.RemoveConnection(userId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, NotificationDTO notification)
        {
            var connectionId = _connectionManager.GetConnectionId(userId);
            Console.WriteLine($"A notification created by {notification.UserID} is being sent to {userId}.");
            Console.WriteLine($"The notification contains the message {notification.Message}");


            Console.WriteLine($"Found profile picture with content type {notification.UserProfilePictureContentType} with data {notification.UserProfilePictureData}");

            try
            {
                await Clients.Client(connectionId)
                .SendAsync("ReceiveMessage", notification);
                Console.WriteLine($"Notification sent to connection {connectionId}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in SendNotification: {e.Message}");
                throw;
            }
        }
    }
}

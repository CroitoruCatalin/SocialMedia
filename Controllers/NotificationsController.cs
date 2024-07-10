using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using SocialMedia.Services.Interfaces;
using SQLitePCL;

namespace SocialMedia.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly SocialContext _context;

        public NotificationsController(
            INotificationService notificationService, 
            UserManager<User> userManager,
            SocialContext context)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = _userManager.GetUserId(User);
            List<Notification> notifications = [];
            if (userId != null)
            {
                notifications = await _notificationService.GetNotificationsForUserAsync(userId);
            }
            return Ok(notifications);
        }


        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead([FromBody] NotificationIdDto notificationIdDto)
        {
            if (notificationIdDto.NotificationId <= 0)
            {
                return BadRequest("Invalid notification ID.");
            }
            var notification = await _context.Notifications.FindAsync(notificationIdDto.NotificationId);
            if(notification != null)
            {
                notification.IsRead = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}

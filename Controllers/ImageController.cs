using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly SocialContext _context;

        public ImageController(SocialContext context)
        {
            _context = context;
        }

        [HttpGet("GetProfilePicture/{userId}")]
        public async Task<IActionResult> GetProfilePicture(string userId)
        {
            var user = await _context.Users
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.ProfilePicture == null)
            {
                return NotFound();
            }

            var image = user.ProfilePicture;
            var base64String = Convert.ToBase64String(image.Data);
            var imageDataUrl = $"data:{image.ContentType};base64,{base64String}";

            return Ok(imageDataUrl);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Services;
using System.Threading.Tasks;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ImageService _imageService;

    public ProfileController(UserManager<User> userManager, ImageService imageService)
    {
        _userManager = userManager;
        _imageService = imageService;
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(IFormFile profilePicture)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (profilePicture != null)
        {
            var image = await _imageService.UploadImageAsync(profilePicture, user.Id);
            user.ProfilePictureId = image.ImageId;
            await _userManager.UpdateAsync(user);
        }

        return RedirectToAction("EditProfile");
    }
}

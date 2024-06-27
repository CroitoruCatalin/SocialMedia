using SocialMedia.Models;

namespace SocialMedia.Services
{
    public class ImageService
    {
        private readonly SocialContext _context;

        public ImageService(SocialContext context)
        {
            _context = context;
        }

        public async Task<Image> UploadImageAsync(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var image = new Image
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = memoryStream.ToArray(),
                    UserId = userId,
                    UploadDate = DateTime.UtcNow
                };
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                return image;
            }
        }
        public async Task<Image> AssignDefaultProfilePicture(string userId)
        {
            var defaultImage = new Image
            {
                FileName = "default.png",
                ContentType = "image/png",
                Data = await File.ReadAllBytesAsync("wwwroot/assets/images/default.png"), // Assume you have a default image in this path
                UserId = userId,
                UploadDate = DateTime.UtcNow
            };
            _context.Images.Add(defaultImage);
            await _context.SaveChangesAsync();
            return defaultImage;
        }
    }
}

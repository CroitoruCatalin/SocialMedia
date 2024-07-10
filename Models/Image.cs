using SocialMedia.Models.ViewModels;

namespace SocialMedia.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string FileName { get; set; } = String.Empty;
        public string ContentType { get; set; } = String.Empty;
        public byte[] Data { get; set; } = [];
        public string UserId { get; set; } = String.Empty;
        public DateTime UploadDate { get; set; } = DateTime.MinValue;

        public void SetFromImageViewModel(ImageViewModel model, string userId)
        {
            FileName = String.Empty;
            ContentType = model.ContentType;
            if (!string.IsNullOrEmpty(model.Data))
            {
                try
                {
                    Data = Convert.FromBase64String(model.Data);
                }
                catch (FormatException ex)
                {
                    throw new FormatException("Invalid Base64 string for image data.", ex);
                }
            }
            UserId = userId;
            UploadDate = DateTime.UtcNow;
        }

    }
}

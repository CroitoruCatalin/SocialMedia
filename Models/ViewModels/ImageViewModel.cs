namespace SocialMedia.Models.ViewModels
{
    public class ImageViewModel
    {
        public int ImageId { get; set; } = new int();
        public string ContentType { get; set; } = String.Empty;
        public string Data { get; set; } = String.Empty;

        public ImageViewModel(Image image) {
            ImageId = image.ImageId;
            ContentType = image.ContentType;
            Data = Convert.ToBase64String(image.Data);
        }

        public ImageViewModel()
        {
            ImageId = new int();
            ContentType = "NO_CONTENT";
            Data = "NO_DATA";
        }

        public string GetDetails(string caller)
        {
            return "ImageViewModel in "+caller+" has:\nImageId="+ImageId+"\nContentType="+ContentType+"\nData="+Data;
        }
    }
}

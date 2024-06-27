namespace SocialMedia.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime UploadDate { get; set; }
    }
}

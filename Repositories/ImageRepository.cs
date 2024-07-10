using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public ImageRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }


        public async Task AddImageAsync(Image image)
        {
            await _SocialContext.Images.AddAsync(image);
        }

        public void DeleteImage(Image image)
        {
            _SocialContext.Images.Remove(image);
        }

        public async Task<Image?> GetImageByIdAsync(int id)
        {
            return await _SocialContext.Images
                .FirstOrDefaultAsync(i => i.ImageId == id);
        }

        public Task<ImageViewModel?> GetImageViewModelAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateImageAsync(Image image)
        {
            _SocialContext.Images.Update(image);
        }
    }
}

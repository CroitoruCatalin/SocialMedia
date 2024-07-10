using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
namespace SocialMedia.Repositories.Interfaces
{
    public interface IImageRepository : IRepositoryBase<Image>
    {
        Task<Image?> GetImageByIdAsync(int id);
        Task<ImageViewModel?> GetImageViewModelAsync(int id);
        Task AddImageAsync(Image image);
        Task UpdateImageAsync(Image image);
        void DeleteImage(Image image);
    }
}

using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class PostLikeRepository : RepositoryBase<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }

    }
}

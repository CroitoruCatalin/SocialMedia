using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class CommentLikeRepository : RepositoryBase<CommentLike>, ICommentLikeRepository
    {
        public CommentLikeRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }

    }
}

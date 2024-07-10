using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }
        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            return await _SocialContext.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .FirstOrDefaultAsync(p => p.ID == postId);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _SocialContext.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .ToListAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            await _SocialContext.Posts.AddAsync(post);
        }

        public async Task UpdatePostAsync(Post post)
        {
            _SocialContext.Posts.Update(post);
        }

        public async Task DeletePostAsync(Post post)
        {
            var _post = await _SocialContext.Posts.FindAsync(post.ID);
            if (_post != null)
            {
                _SocialContext.Posts.Remove(_post);
            }
        }

        public async Task<PostPresentationViewModel> GetPostPresentationWithTopCommentAsync(int postId)
        {
            Console.WriteLine("Starting GetPostPresentation(" + postId + ") from PostRepository");

            var post = await _SocialContext.Posts
            .Include(p => p.Comments)
                .ThenInclude(c => c.Reactions)
            .Include(p => p.Image)
            .Include(p => p.Reactions)
            .Include(p => p.User)
                .ThenInclude(u => u.ProfilePicture)
            .FirstOrDefaultAsync(p => p.ID == postId);


            PostPresentationViewModel result = new PostPresentationViewModel();

            if (post != null)
            {
                result = new PostPresentationViewModel(post);
                var topComment = post.Comments.OrderByDescending(c => c.CreationDate).FirstOrDefault();
                if(topComment != null)
                {
                    var topCommentAuthor = await _SocialContext.Users.FirstOrDefaultAsync(u => u.Id == topComment.UserID);
                    if(topCommentAuthor != null)
                    {
                        topComment.User = topCommentAuthor;
                    }


                    result.TopComment = new CommentPresentationViewModel(topComment);
                }
            }
            else
            {

            }

            

            Console.WriteLine("Returning the following post presentation:\n" + result.GetDetails("PostRepository"));

            return result;
        }

        public async Task<IEnumerable<int>> GetUserPostsAsync(string userId)
        {
            return await _SocialContext.Posts
                .Where(p => p.UserID == userId)
                .OrderByDescending(p => p.CreationDate)
                .Select(p => p.ID)
                .ToListAsync();
        }
    }
}

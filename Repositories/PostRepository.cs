using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(SocialContext socialContext)
            : base(socialContext)
        {
        }
        public async Task<Post> GetPostByIdAsync(int postId)
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
            await _SocialContext.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _SocialContext.Posts.Update(post);
            await _SocialContext.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Post post)
        {
            var _post = await _SocialContext.Posts.FindAsync(post.ID);
            if (_post != null)
            {
                _SocialContext.Posts.Remove(_post);
                await _SocialContext.SaveChangesAsync();
            }
        }
    }
}

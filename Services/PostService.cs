using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;
using System.Security.Claims;

namespace SocialMedia.Services
{
    public class PostService : IPostService
    {
        private readonly SocialContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        public PostService(
            SocialContext context,
            IRepositoryWrapper repositoryWrapper, 
            UserManager<User> userManager)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }
        public async Task CreatePostAsync(Post post, ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if(user != null)
            {
                post.UserID = user.Id;
                post.User = user;
                post.CreationDate = DateTime.UtcNow;
                post.ModifiedDate = post.CreationDate;
  

                _repositoryWrapper.PostRepository.Create(post);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _repositoryWrapper.PostRepository
                .FindByCondition(p => p.ID == postId)
                .FirstOrDefaultAsync();
            if (post != null)
            {
                _repositoryWrapper.PostRepository.Delete(post);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public List<Post> GetAllPosts()
        {
            int pageNumber = 1;
            int pageSize = 20;

            return _repositoryWrapper
                .PostRepository
                .FindAll()
                .Include(p => p.User)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                .OrderByDescending(p => p.CreationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _repositoryWrapper
                .PostRepository
                .FindAll()
                .OrderByDescending(p => p.CreationDate)
                .Include(p => p.User)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Reactions)
                .Include(p => p.Comments.OrderByDescending(c => c.CreationDate).Take(1)) // Only fetch the most recent comment
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                .Take(20)
                .ToListAsync();
        }

        public Post GetPostWithComments(int postId)
        {
            var postQuery = _repositoryWrapper
                .PostRepository
                .FindByCondition(p => p.ID == postId)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Reactions);
            return postQuery.FirstOrDefault();
        }


        public async Task<Post?> GetPostById(int postId)
        {
            return await _context.Posts
               .Include(p => p.Reactions)
               .FirstOrDefaultAsync(p => p.ID == postId);
        }

        public async Task UpdatePostAsync(Post post)
        {
            _repositoryWrapper.PostRepository.Update(post);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _repositoryWrapper.PostRepository
                .GetPostByIdAsync(postId);
        }
    }
}

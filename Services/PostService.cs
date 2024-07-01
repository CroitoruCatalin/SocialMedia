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

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        public PostService(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }
        public async Task CreatePost(Post post, ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if(user != null)
            {
                post.UserID = user.Id;
                post.User = user;
                post.CreationDate = DateTime.UtcNow;

                _repositoryWrapper.PostRepository.Create(post);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task DeletePost(int postId)
        {
            var post = await _repositoryWrapper.PostRepository
                .FindByCondition(p => p.PostID == postId)
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
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _repositoryWrapper
                .PostRepository
                .FindAll()
                .OrderByDescending(p => p.CreationDate)
                .Include(p => p.User)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Comments.OrderByDescending(c => c.CreationDate).Take(1)) // Only fetch the most recent comment
                    .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                .Take(20)
                .ToListAsync();
        }

        public Post GetPostWithComments(int postId)
        {
            var postQuery = _repositoryWrapper
                .PostRepository
                .FindByCondition(p => p.PostID == postId)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Likes);
            return postQuery.FirstOrDefault();
        }


        public async Task<Post?> GetPostById(int postId)
        {
            return await _repositoryWrapper.PostRepository
                .FindByCondition(p => p.PostID == postId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePost(Post post)
        {
            _repositoryWrapper.PostRepository.Update(post);
            await _repositoryWrapper.SaveAsync();
        }

    }
}

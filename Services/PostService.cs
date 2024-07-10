using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Models.ViewModels;
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

                Console.WriteLine("ServiceCreatePostAsync post.UserID: " + post.UserID);
                Console.WriteLine("ServiceCreatePostAsync post.User.FullName: " + post.User.FullName);
                user.Posts.Add(post);

                if(post.Image != null)
                {
                    await _repositoryWrapper.ImageRepository.AddImageAsync(post.Image);
                }

                await _repositoryWrapper.PostRepository.AddPostAsync(post);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task DeletePostAsync(int postId)
        {
            //find the post
            var post = await _repositoryWrapper.PostRepository
                .FindByCondition(p => p.ID == postId)
                .Include(p => p.Image)
                .Include(p => p.Reactions)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Image)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Reactions)
                .FirstOrDefaultAsync();

            //if post exists you can delete it
            if (post != null)
            {
                //get rid of post reactions
                foreach (var reaction in post.Reactions)
                {
                    await _repositoryWrapper.ReactionRepository.RemoveReactionAsync(reaction.ReactionID);
                }
                //get rid of comments on the post
                foreach (var comment in post.Comments)
                {
                    //get read of comment reactions
                    foreach(var reaction in comment.Reactions)
                    {
                        await _repositoryWrapper.ReactionRepository.RemoveReactionAsync(reaction.ReactionID);
                    }

                    //get rid of comment image
                    if (comment.Image != null)
                    {
                        _repositoryWrapper.ImageRepository.DeleteImage(comment.Image);
                    }
                    //delete comment
                    await _repositoryWrapper.CommentRepository.DeleteCommentAsync(comment.ID);
                }
                //delete post image
                if(post.Image != null)
                { 
                    _repositoryWrapper.ImageRepository.DeleteImage(post.Image);
                }
                await _repositoryWrapper.PostRepository.DeletePostAsync(post);
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

        public async Task<Post> GetPostWithCommentsAsync(int postId)
        {
            var post = await _repositoryWrapper
                .PostRepository
                .FindByCondition(p => p.ID == postId)
                .Include(p => p.User)
                .ThenInclude(u => u.ProfilePicture)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Reactions)
                .FirstOrDefaultAsync();

            post ??= new Post();

            return post;
        }


        public async Task<Post?> GetPostById(int postId)
        {
            return await _repositoryWrapper.PostRepository
                .GetPostByIdAsync(postId);
        }

        public async Task UpdatePostAsync(Post post)
        {
            _repositoryWrapper.PostRepository.Update(post);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            return await _repositoryWrapper.PostRepository
                .GetPostByIdAsync(postId);
        }

        public async Task<IEnumerable<int>> GetPostRecommendationsAsync(string userId)
        {
            return await _context.Posts
                .OrderByDescending(p => p.ModifiedDate)
                .Select(p => p.ID)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> RecommendationAlgorithm(string userId)
        {
            var followingIds = await _context.UserUsers
                .Where(uu => uu.UserId == userId)
                .Select(uu => uu.FriendId)
                .ToListAsync();

            var recommendations = await _context.Posts
                .Include(p => p.Reactions)
                .Include(p => p.User)
                .Select(p => new
                {
                    PostId = p.ID,
                    IsFromFollowedUser = followingIds.Contains(p.UserID),
                    LikeCount = p.LikeCount,
                    CreationDate = p.CreationDate,
                    CommentCount = p.CommentCount
                })
                .OrderByDescending(p => p.IsFromFollowedUser)
                .ThenByDescending(p => p.CreationDate)
                .ThenByDescending(p => p.LikeCount)
                .ThenByDescending(p => p.CommentCount)
                .Select(p => p.PostId)
                .ToListAsync();

            return recommendations;
        }

        public async Task<PostPresentationViewModel> GetPostPresentation(int postId)
        {
            Console.WriteLine("Starting GetPostPresentation(" + postId + ") from PostService");
            return await _repositoryWrapper.PostRepository.GetPostPresentationWithTopCommentAsync(postId);
        }

        public Task<IEnumerable<int>> GetPostIdsForUserProfileAsync(string userId)
        {
            return _repositoryWrapper.PostRepository.GetUserPostsAsync(userId);
        }
    }
}

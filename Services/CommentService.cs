using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using SocialMedia.Services.Interfaces;

namespace SocialMedia.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public CommentService(
            IRepositoryWrapper repositoryWrapper,
            INotificationService notificationService,
            UserManager<User> userManager
            )
        {
            _repositoryWrapper = repositoryWrapper;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == commentId)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _repositoryWrapper.CommentRepository
                .FindAll()
                .ToListAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            //get parent post of the coment
            Post post = await _repositoryWrapper.PostRepository.GetPostByIdAsync(comment.PostID);
            comment.Post = post;

            //get author of the parrent post
            string postAuthorId = post.UserID;
            User postAuthor = await _repositoryWrapper.UserRepository.GetUserByIdAsync(postAuthorId);
            User commentAuthor = await _repositoryWrapper.UserRepository.GetUserByIdAsync(comment.UserID);


            //create notification if the user leaves a comment on another user's post
            if (postAuthorId != comment.UserID)
            {
                Notification notification = new Notification
                {
                    UserID = postAuthorId,
                    User = postAuthor,
                    UserName = commentAuthor.FullName,
                    Message ="commented on one of your posts!",
                    Type = NotificationType.NewComment,
                    SourceID = post.ID,
                    InstigatorID = comment.UserID,
                    Url = $"/Posts/Details/{comment.PostID}"
                };
                postAuthor.Notifications.Add(notification);
                _repositoryWrapper.UserRepository.Update(postAuthor);

  
                await _notificationService.SendNotificationAsync(postAuthorId, notification);
            }
          
            post.CommentCount++;
            post.Comments.Add(comment);
            _repositoryWrapper.CommentRepository.Create(comment);
            _repositoryWrapper.PostRepository.Update(post);

            await _repositoryWrapper.SaveAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _repositoryWrapper.CommentRepository.Update(comment);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == commentId)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                _repositoryWrapper.CommentRepository.Delete(comment);
                await _repositoryWrapper.SaveAsync();
            }
        }
        public async Task<bool> CommentExists(int id)
        {
            return await _repositoryWrapper.CommentRepository
                .FindByCondition(c => c.ID == id).AnyAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _repositoryWrapper.CommentRepository
                .GetCommentsByPostIdAsync(postId);
        }
    }
}

using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;

namespace SocialMedia.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public SocialContext _socialContext;
        public IUserRepository? _userRepository;
        public IPostRepository? _postRepository;
        public ICommentRepository? _commentRepository;
        public IReactionRepository? _reactionRepository;
        public INotificationRepository? _notificationRepository;
        public IImageRepository? _imageRepository;
        public RepositoryWrapper(SocialContext socialContext)
        {
            _socialContext = socialContext;
        }

        public IUserRepository UserRepository
        {       
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_socialContext);
                }

                return _userRepository;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {
                if (_postRepository == null)
                {
                    _postRepository = new PostRepository(_socialContext);
                }

                return _postRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_socialContext);
                }

                return _commentRepository;
            }
        }

        public IReactionRepository ReactionRepository
        {
            get
            {
                if (_reactionRepository == null)
                {
                    _reactionRepository = new ReactionRepository(_socialContext);
                }

                return _reactionRepository;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if(_notificationRepository == null)
                {
                    _notificationRepository = new NotificationRepository(_socialContext);
                }
                return _notificationRepository;
            }
        }
        public IImageRepository ImageRepository
        {
            get
            {
                if (_imageRepository == null)
                {
                    _imageRepository = new ImageRepository(_socialContext);
                }
                return _imageRepository;
            }
        }

        public void Save()
        {
            _socialContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _socialContext.SaveChangesAsync();
        }
    }
}

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
        public IPostLikeRepository? _postLikeRepository;
        public ICommentLikeRepository? _commentLikeRepository;
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

        public IPostLikeRepository PostLikeRepository
        {
            get
            {
                if (_postLikeRepository == null)
                {
                    _postLikeRepository = new PostLikeRepository(_socialContext);
                }

                return _postLikeRepository;
            }
        }

        public ICommentLikeRepository CommentLikeRepository
        {
            get
            {
                if (_commentLikeRepository == null)
                {
                    _commentLikeRepository = new CommentLikeRepository(_socialContext);
                }

                return _commentLikeRepository;
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

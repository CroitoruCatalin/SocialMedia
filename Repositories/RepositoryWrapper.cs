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

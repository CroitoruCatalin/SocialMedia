namespace SocialMedia.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        ICommentRepository CommentRepository { get; }
        IPostLikeRepository PostLikeRepository { get; }
        ICommentLikeRepository CommentLikeRepository { get; }

        void Save();
        Task SaveAsync();
    }
}

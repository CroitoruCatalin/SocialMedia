namespace SocialMedia.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        ICommentRepository CommentRepository { get; }
        IReactionRepository ReactionRepository { get; }

        void Save();
        Task SaveAsync();
    }
}

namespace SocialMedia.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPostRepository PostRepository { get; }
        ICommentRepository CommentRepository { get; }
        IReactionRepository ReactionRepository { get; }
        INotificationRepository NotificationRepository { get; }

        void Save();
        Task SaveAsync();
    }
}

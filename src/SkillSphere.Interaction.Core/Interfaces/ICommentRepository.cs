namespace SkillSphere.Interaction.Core.Interfaces;

public interface ICommentRepository
{
    IAsyncEnumerable<Comment> GetPostComments(Guid postId);

    Task<Comment?> GetCommentById(Guid commentId);

    Task AddComment(Comment comment);

    Task UpdateComment(Comment comment);

    Task RemoveComment(Comment comment);
}

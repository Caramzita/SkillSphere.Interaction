using MediatR;
using SkillSphere.Infrastructure.UseCases;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.RemoveComment;

public class RemoveCommentCommand : IRequest<Result<Unit>>
{
    public Guid CommentId { get; }

    public Guid PostId { get; }

    public RemoveCommentCommand(Guid commentId, Guid postId)
    {
        CommentId = commentId;
        PostId = postId;
    }
}

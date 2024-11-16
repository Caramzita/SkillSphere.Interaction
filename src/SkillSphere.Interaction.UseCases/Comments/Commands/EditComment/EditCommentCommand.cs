using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.EditComment;

public class EditCommentCommand : IRequest<Result<Comment>>, ICommentCommand
{
    public Guid CommentId { get; }

    public Guid PostId { get; }

    public string Content { get; } = string.Empty;

    public EditCommentCommand(Guid commentId, Guid postId, string content)
    {
        CommentId = commentId;
        PostId = postId;
        Content = content;
    }
}

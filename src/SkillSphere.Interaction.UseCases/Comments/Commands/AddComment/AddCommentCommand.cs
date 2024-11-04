using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.AddComment;

public class AddCommentCommand : IRequest<Result<Comment>>
{
    public Guid PostId { get; }

    public Guid UserId { get; }

    public string Content { get; } = string.Empty;

    public AddCommentCommand(Guid postId, Guid userId, string content)
    {
        PostId = postId;
        UserId = userId;
        Content = content;
    }
}

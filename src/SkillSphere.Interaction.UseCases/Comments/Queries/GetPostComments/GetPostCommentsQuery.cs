using MediatR;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace SkillSphere.Interaction.UseCases.Comments.Queries.GetPostComments;

public record GetPostCommentsQuery(Guid PostId) : IStreamRequest<Comment>;

public class GetPostCommentsQueryHandler : IStreamRequestHandler<GetPostCommentsQuery, Comment>
{
    private readonly ICommentRepository _commentRepository;

    public GetPostCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }

    public async IAsyncEnumerable<Comment> Handle(GetPostCommentsQuery request, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var comments = _commentRepository.GetPostComments(request.PostId);

        await foreach (var comment in comments.WithCancellation(cancellationToken))
        {
            yield return comment;
        }
    }
}

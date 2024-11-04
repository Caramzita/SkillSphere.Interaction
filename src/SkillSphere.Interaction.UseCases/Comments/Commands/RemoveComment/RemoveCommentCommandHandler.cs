using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.RemoveComment;

public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Result<Unit>>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IPostService _entityService;

    public RemoveCommentCommandHandler(ICommentRepository commentRepository, IPostService entityService)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
    }

    public async Task<Result<Unit>> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var postExists = await _entityService.PostExists(request.PostId);

            if (!postExists)
            {
                return Result<Unit>.Invalid("Post wasn't found");
            }

            var comment = await _commentRepository.GetCommentById(request.CommentId);

            if (comment == null)
            {
                return Result<Unit>.Invalid("Comment wasn't found");
            }

            await _commentRepository.RemoveComment(comment);

            return Result<Unit>.Empty();
        }
        catch (Exception)
        {
            return Result<Unit>.Invalid("An error occurred while removing the comment.");
        }
    }
}

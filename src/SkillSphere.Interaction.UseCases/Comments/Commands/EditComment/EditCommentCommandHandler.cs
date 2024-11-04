using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.EditComment;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, Result<Comment>>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IPostService _entityService;

    public EditCommentCommandHandler(ICommentRepository commentRepository, IPostService entityService)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
    }

    public async Task<Result<Comment>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var postExists = await _entityService.PostExists(request.PostId);

            if (!postExists)
            {
                return Result<Comment>.Invalid("Post wasn't found");
            }

            var comment = await _commentRepository.GetCommentById(request.CommentId);

            if (comment == null)
            {
                return Result<Comment>.Invalid("Comment wasn't found");
            }

            comment.UpdateContent(request.Content);
            await _commentRepository.UpdateComment(comment);

            return Result<Comment>.Success(comment);
        }
        catch (Exception)
        {
            return Result<Comment>.Invalid("An error occurred while editing the comment.");
        }
    }
}

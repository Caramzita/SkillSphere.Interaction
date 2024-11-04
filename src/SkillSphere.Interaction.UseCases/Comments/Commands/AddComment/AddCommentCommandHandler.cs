using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Comments.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<Comment>>
{
    private readonly ICommentRepository _commentRepository;

    private readonly IPostService _entityService;

    public AddCommentCommandHandler(ICommentRepository commentRepository, IPostService entityService)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
    }

    public async Task<Result<Comment>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var postExists = await _entityService.PostExists(request.PostId);

            if (!postExists)
            {
                return Result<Comment>.Invalid("Post wasn't found");
            }

            var comment = new Comment(request.PostId, request.UserId, request.Content);

            await _commentRepository.AddComment(comment);

            return Result<Comment>.Success(comment);
        }    
        catch (Exception)
        {
            return Result<Comment>.Invalid("An error occurred while adding the comment.");
        }
    }
}

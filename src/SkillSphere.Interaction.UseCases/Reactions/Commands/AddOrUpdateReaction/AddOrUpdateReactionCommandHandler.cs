using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Enums;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Reactions.Commands.AddOrUpdateReaction;

public class AddOrUpdateReactionCommandHandler : IRequestHandler<AddOrUpdateReactionCommand, Result<Reaction>>
{
    private readonly IReactionRepository _reactionRepository;

    private readonly IPostService _entityService;

    private readonly ICommentRepository _commentRepository;

    public AddOrUpdateReactionCommandHandler(IReactionRepository reactionRepository, 
        IPostService entityService, ICommentRepository commentRepository)
    {
        _reactionRepository = reactionRepository ?? throw new ArgumentNullException(nameof(reactionRepository));
        _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }

    public async Task<Result<Reaction>> Handle(AddOrUpdateReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entityExists = request.EntityType switch
            {
                EntityType.Post => await _entityService.PostExists(request.EntityId),
                EntityType.Comment => (await _commentRepository.GetCommentById(request.EntityId)) != null,
                _ => false
            };

            if (!entityExists)
            {
                return Result<Reaction>.Invalid($"Сущность {request.EntityType} с ID {request.EntityId} не найдена.");
            }

            var existingReaction = await _reactionRepository.GetReactionByUserAndEntity(
                request.UserId, request.EntityId, request.EntityType);

            if (existingReaction != null)
            {
                if (existingReaction.ReactionType != request.ReactionType)
                {
                    existingReaction.ChangeReactionType(request.ReactionType);
                    await _reactionRepository.UpdateReaction(existingReaction);
                }

                return Result<Reaction>.Success(existingReaction);
            }

            var newReaction = new Reaction(
            request.UserId,
            request.EntityId,
            request.EntityType,
            request.ReactionType);

            await _reactionRepository.AddReaction(newReaction);

            return Result<Reaction>.Success(newReaction);
        }
        catch (Exception)
        {
            return Result<Reaction>.Invalid("An error occurred while adding or updating the reaction.");
        }
    }
}

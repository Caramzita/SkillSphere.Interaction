using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Interfaces;

namespace SkillSphere.Interaction.UseCases.Reactions.Commands.RemoveReaction;

public class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, Result<Unit>>
{
    private readonly IReactionRepository _reactionRepository;

    public RemoveReactionCommandHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository ?? throw new ArgumentNullException(nameof(reactionRepository));
    }

    public async Task<Result<Unit>> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reaction = await _reactionRepository.GetReactionById(
                request.ReactionId, request.EntityType, request.EntityId);

            if (reaction == null)
            {
                return Result<Unit>.Invalid($"Реакция {request.ReactionId} не найдена");
            }

            await _reactionRepository.RemoveReaction(reaction);

            return Result<Unit>.Empty();
        }
        catch (Exception)
        {
            return Result<Unit>.Invalid("An error occurred while removing the reaction.");
        }
    }
}

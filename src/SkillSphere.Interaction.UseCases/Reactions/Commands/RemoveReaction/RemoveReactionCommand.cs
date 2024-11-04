using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core.Enums;

namespace SkillSphere.Interaction.UseCases.Reactions.Commands.RemoveReaction;

public class RemoveReactionCommand : IRequest<Result<Unit>>
{
    public Guid ReactionId { get; }

    public Guid EntityId { get; set; }

    public EntityType EntityType { get; set; }

    public Guid UserId { get; }

    public RemoveReactionCommand(Guid reactionId, Guid entityId, Guid userId, EntityType entityType)
    {
        ReactionId = reactionId;
        EntityId = entityId;
        EntityType = entityType;
        UserId = userId;
    }
}

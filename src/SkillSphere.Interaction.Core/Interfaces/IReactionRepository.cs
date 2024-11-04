using SkillSphere.Interaction.Core.Enums;

namespace SkillSphere.Interaction.Core.Interfaces;

public interface IReactionRepository
{
    IAsyncEnumerable<Reaction> GetEntityReactions(Guid entityId, EntityType entityType);

    Task<Reaction?> GetReactionByUserAndEntity(Guid userId, Guid entityId, EntityType entityType);

    Task<Reaction?> GetReactionById(Guid reactionId, EntityType type, Guid entityId);

    Task AddReaction (Reaction reaction);

    Task UpdateReaction(Reaction reaction);

    Task RemoveReaction (Reaction reaction);
}

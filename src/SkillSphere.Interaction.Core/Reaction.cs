using SkillSphere.Interaction.Core.Enums;

namespace SkillSphere.Interaction.Core;

public class Reaction
{
    public Guid Id { get; init; }

    public Guid UserId { get; private set; }

    public Guid EntityId { get; private set; }

    public EntityType EntityType {  get; private set; }

    public ReactionType ReactionType { get; private set; }

    public DateTime CreatedAt {  get; init; }

    public Reaction(Guid userId, Guid entityId, 
        EntityType entityType, ReactionType reactionType)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
        ReactionType = reactionType;
        CreatedAt = DateTime.UtcNow;
    }

    public Reaction(Guid id, Guid userId, Guid entityId,
        EntityType entityType, ReactionType reactionType, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
        ReactionType = reactionType;
        CreatedAt = createdAt;
    }

    public void ChangeReactionType(ReactionType reactionType)
    {
        ReactionType = reactionType;
    }
}

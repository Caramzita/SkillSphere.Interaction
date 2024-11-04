using MediatR;
using SkillSphere.Infrastructure.UseCases;
using SkillSphere.Interaction.Core;
using SkillSphere.Interaction.Core.Enums;

namespace SkillSphere.Interaction.UseCases.Reactions.Commands.AddOrUpdateReaction;

public class AddOrUpdateReactionCommand : IRequest<Result<Reaction>>
{
    public Guid UserId { get; set; }

    public Guid EntityId { get; set; }

    public EntityType EntityType { get; set; }

    public ReactionType ReactionType { get; set; }
}

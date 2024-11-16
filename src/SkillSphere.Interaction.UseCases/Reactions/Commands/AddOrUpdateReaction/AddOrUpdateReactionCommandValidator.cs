using FluentValidation;

namespace SkillSphere.Interaction.UseCases.Reactions.Commands.AddOrUpdateReaction;

public class AddOrUpdateReactionCommandValidator : AbstractValidator<AddOrUpdateReactionCommand>
{
    public AddOrUpdateReactionCommandValidator()
    {
        RuleFor(command => command.ReactionType)
            .IsInEnum().WithMessage("Reaction type must be a valid value.");

        RuleFor(command => command.EntityType)
            .IsInEnum().WithMessage("Entity type must be a valid value.");
    }
}

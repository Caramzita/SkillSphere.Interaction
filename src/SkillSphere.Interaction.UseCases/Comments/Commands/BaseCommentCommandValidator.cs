using FluentValidation;

namespace SkillSphere.Interaction.UseCases.Comments.Commands;

public class BaseCommentCommandValidator<T> : AbstractValidator<T> 
    where T : ICommentCommand
{
    public BaseCommentCommandValidator()
    {
        RuleFor(command => command.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(250).WithMessage("Content must not exceed 250 characters.");
    }
}

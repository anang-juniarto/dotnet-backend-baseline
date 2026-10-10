using FluentValidation;

namespace Baseline.Sample.Application.Features.Items.Commands.CreateItem;

/// <summary>Rejects missing actors and names outside the normalized domain boundary.</summary>
public sealed class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    /// <summary>Defines the same normalized name invariant enforced by the domain.</summary>
    public CreateItemCommandValidator()
    {
        RuleFor(command => command.OwnerId).NotEmpty();
        RuleFor(command => command.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length <= 100)
            .WithMessage("Name must contain 1 to 100 non-whitespace characters after trimming.");
    }
}

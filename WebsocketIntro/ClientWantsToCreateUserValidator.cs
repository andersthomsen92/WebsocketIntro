using FluentValidation;
using WebsocketIntro.EventHandlers;

namespace WebsocketIntro;

public class ClientWantsToCreateUserValidator : AbstractValidator<ClientWantsToCreateUser>
{
    public ClientWantsToCreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("User name cannot be empty")
            .NotNull().WithMessage("User name cannot be null")
            .MinimumLength(3).WithMessage("User name must be at least 3 characters long");
    }
    
}
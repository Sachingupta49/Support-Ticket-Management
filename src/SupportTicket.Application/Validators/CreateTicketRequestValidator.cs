using FluentValidation;
using SupportTicket.Application.DTOs;

namespace SupportTicket.Application.Validators;

public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
  private static readonly string[] ValidPriorities = ["Low", "Medium", "High", "Critical"];

    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required.")
            .Must(p => ValidPriorities.Contains(p, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Priority must be one of: Low, Medium, High, Critical.");

        RuleFor(x => x.AssigneeId)
            .GreaterThan(0).WithMessage("AssigneeId is required.");
    }
}

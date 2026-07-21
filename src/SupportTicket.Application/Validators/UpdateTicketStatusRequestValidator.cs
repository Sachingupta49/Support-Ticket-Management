using FluentValidation;
using SupportTicket.Application.DTOs;

namespace SupportTicket.Application.Validators;

public class UpdateTicketStatusRequestValidator : AbstractValidator<UpdateTicketStatusRequest>
{
    private static readonly string[] ValidStatuses =
        ["Open", "InProgress", "Resolved", "Closed", "Cancelled"];

    public UpdateTicketStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => ValidStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status must be one of: Open, InProgress, Resolved, Closed, Cancelled.");
    }
}

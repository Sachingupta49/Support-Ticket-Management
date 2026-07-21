using FluentValidation;
using SupportTicket.Application.DTOs;

namespace SupportTicket.Application.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("AuthorId is required.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Comment body is required.")
            .MaximumLength(2000).WithMessage("Comment body must not exceed 2000 characters.");
    }
}

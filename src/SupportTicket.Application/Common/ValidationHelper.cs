using FluentValidation;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Exceptions;

namespace SupportTicket.Application.Common;

public static class ValidationHelper
{
    public static async Task ValidateAsync<T>(IValidator<T> validator, T instance, CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);
        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            throw new AppValidationException(errors);
        }
    }
}

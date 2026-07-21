using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Enums;
using SupportTicket.Domain.Exceptions;

namespace SupportTicket.Application.Services;

public class StatusTransitionValidator : IStatusTransitionValidator
{
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions = new()
    {
        [TicketStatus.Open] = [TicketStatus.InProgress, TicketStatus.Cancelled],
        [TicketStatus.InProgress] = [TicketStatus.Resolved, TicketStatus.Cancelled],
        [TicketStatus.Resolved] = [TicketStatus.Closed],
        [TicketStatus.Closed] = [],
        [TicketStatus.Cancelled] = []
    };

    public void Validate(TicketStatus currentStatus, TicketStatus newStatus)
    {
        if (currentStatus == newStatus)
        {
            throw new InvalidStatusTransitionException(
                $"Ticket is already in {currentStatus} status.");
        }

        var allowed = GetAllowedTransitions(currentStatus);
        if (!allowed.Contains(newStatus))
        {
            var allowedNames = allowed.Count > 0
                ? string.Join(", ", allowed)
                : "none";

            throw new InvalidStatusTransitionException(
                $"Cannot transition from {currentStatus} to {newStatus}. Allowed: {allowedNames}.");
        }
    }

    public IReadOnlyList<TicketStatus> GetAllowedTransitions(TicketStatus currentStatus) =>
        AllowedTransitions.TryGetValue(currentStatus, out var transitions)
            ? transitions
            : [];
}

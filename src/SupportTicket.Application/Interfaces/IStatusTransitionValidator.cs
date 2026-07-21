using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Interfaces;

public interface IStatusTransitionValidator
{
  void Validate(TicketStatus currentStatus, TicketStatus newStatus);
    IReadOnlyList<TicketStatus> GetAllowedTransitions(TicketStatus currentStatus);
}

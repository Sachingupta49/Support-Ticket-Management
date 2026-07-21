namespace SupportTicket.Application.DTOs;

public record TicketDto(
    int Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    int AssigneeId,
    string AssigneeName,
    DateTime CreatedAt,
    DateTime UpdatedAt);

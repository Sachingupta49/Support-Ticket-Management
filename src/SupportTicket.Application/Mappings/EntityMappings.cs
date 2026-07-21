using SupportTicket.Application.DTOs;
using SupportTicket.Domain.Entities;
using SupportTicket.Domain.Enums;

namespace SupportTicket.Application.Mappings;

public static class EntityMappings
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.Name, user.Email);

    public static TicketDto ToDto(this Ticket ticket) =>
        new(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Status.ToString(),
            ticket.Priority.ToString(),
            ticket.AssigneeId,
            ticket.Assignee?.Name ?? string.Empty,
            ticket.CreatedAt,
            ticket.UpdatedAt);

    public static CommentDto ToDto(this Comment comment) =>
        new(
            comment.Id,
            comment.TicketId,
            comment.AuthorId,
            comment.Author?.Name ?? string.Empty,
            comment.Body,
            comment.CreatedAt);

    public static TicketPriority ParsePriority(string priority) =>
        Enum.Parse<TicketPriority>(priority, ignoreCase: true);

    public static TicketStatus? ParseStatus(string? status) =>
        string.IsNullOrWhiteSpace(status)
            ? null
            : Enum.Parse<TicketStatus>(status, ignoreCase: true);
}

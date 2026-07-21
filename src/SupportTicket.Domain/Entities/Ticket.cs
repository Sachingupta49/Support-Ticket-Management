using SupportTicket.Domain.Enums;

namespace SupportTicket.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public int AssigneeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User Assignee { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

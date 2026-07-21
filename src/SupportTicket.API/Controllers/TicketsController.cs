using Microsoft.AspNetCore.Mvc;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;
using SupportTicket.Domain.Enums;

namespace SupportTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TicketDto>>> GetTickets(
        [FromQuery] string? search,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        TicketStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<TicketStatus>(status, ignoreCase: true, out var statusValue))
            {
                return BadRequest(new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Bad Request",
                    status = 400,
                    detail = $"Invalid status value '{status}'."
                });
            }

            parsedStatus = statusValue;
        }

        var tickets = await _ticketService.GetTicketsAsync(search, parsedStatus, cancellationToken);
        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id, cancellationToken);
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateTicket(
        [FromBody] CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.CreateTicketAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TicketDto>> UpdateTicket(
        int id,
        [FromBody] UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.UpdateTicketAsync(id, request, cancellationToken);
        return Ok(ticket);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<TicketDto>> ChangeStatus(
        int id,
        [FromBody] UpdateTicketStatusRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.ChangeStatusAsync(id, request, cancellationToken);
        return Ok(ticket);
    }
}

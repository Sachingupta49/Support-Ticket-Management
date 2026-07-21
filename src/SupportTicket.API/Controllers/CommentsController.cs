using Microsoft.AspNetCore.Mvc;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Interfaces;

namespace SupportTicket.API.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:int}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetComments(
        int ticketId,
        CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetCommentsByTicketIdAsync(ticketId, cancellationToken);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(
        int ticketId,
        [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentService.CreateCommentAsync(ticketId, request, cancellationToken);
        return CreatedAtAction(nameof(GetComments), new { ticketId }, comment);
    }
}

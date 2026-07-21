using FluentAssertions;
using SupportTicket.Application.Services;
using SupportTicket.Domain.Enums;
using SupportTicket.Domain.Exceptions;

namespace SupportTicket.UnitTests.Services;

public class StatusTransitionValidatorTests
{
    private readonly StatusTransitionValidator _validator = new();

    [Theory]
    [InlineData(TicketStatus.Open, TicketStatus.InProgress)]
    [InlineData(TicketStatus.Open, TicketStatus.Cancelled)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Resolved)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Cancelled)]
    [InlineData(TicketStatus.Resolved, TicketStatus.Closed)]
    public void Validate_AllowedTransition_DoesNotThrow(TicketStatus from, TicketStatus to)
    {
        var act = () => _validator.Validate(from, to);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(TicketStatus.Open, TicketStatus.Resolved)]
    [InlineData(TicketStatus.Open, TicketStatus.Closed)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Open)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Closed)]
    [InlineData(TicketStatus.Resolved, TicketStatus.Open)]
    [InlineData(TicketStatus.Resolved, TicketStatus.InProgress)]
    [InlineData(TicketStatus.Resolved, TicketStatus.Cancelled)]
    [InlineData(TicketStatus.Closed, TicketStatus.InProgress)]
    [InlineData(TicketStatus.Closed, TicketStatus.Open)]
    [InlineData(TicketStatus.Cancelled, TicketStatus.Open)]
    public void Validate_DisallowedTransition_ThrowsInvalidStatusTransitionException(
        TicketStatus from,
        TicketStatus to)
    {
        var act = () => _validator.Validate(from, to);

        act.Should().Throw<InvalidStatusTransitionException>()
            .Which.Message.Should().Contain($"Cannot transition from {from} to {to}");
    }

    [Fact]
    public void Validate_SameStatus_ThrowsInvalidStatusTransitionException()
    {
        var act = () => _validator.Validate(TicketStatus.Open, TicketStatus.Open);

        act.Should().Throw<InvalidStatusTransitionException>()
            .Which.Message.Should().Contain("already in Open");
    }

    [Theory]
    [InlineData(TicketStatus.Open, 2)]
    [InlineData(TicketStatus.InProgress, 2)]
    [InlineData(TicketStatus.Resolved, 1)]
    [InlineData(TicketStatus.Closed, 0)]
    [InlineData(TicketStatus.Cancelled, 0)]
    public void GetAllowedTransitions_ReturnsExpectedCount(TicketStatus status, int expectedCount)
    {
        var transitions = _validator.GetAllowedTransitions(status);

        transitions.Should().HaveCount(expectedCount);
    }
}

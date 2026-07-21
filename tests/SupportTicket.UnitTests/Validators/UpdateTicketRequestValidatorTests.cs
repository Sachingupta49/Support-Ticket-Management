using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Validators;

namespace SupportTicket.UnitTests.Validators;

public class UpdateTicketRequestValidatorTests
{
    private readonly UpdateTicketRequestValidator _validator = new();

    [Fact]
    public async Task Validate_ValidRequest_Passes()
    {
        var request = new UpdateTicketRequest
        {
            Title = "Updated title",
            Description = "Updated description",
            Priority = "Medium",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyTitle_Fails()
    {
        var request = new UpdateTicketRequest
        {
            Title = "",
            Description = "Description",
            Priority = "Low",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_InvalidPriority_Fails()
    {
        var request = new UpdateTicketRequest
        {
            Title = "Title",
            Description = "Description",
            Priority = "Invalid",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Priority");
    }
}

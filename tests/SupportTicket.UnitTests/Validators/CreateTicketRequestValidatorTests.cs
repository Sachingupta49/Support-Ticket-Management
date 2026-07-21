using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Validators;

namespace SupportTicket.UnitTests.Validators;

public class CreateTicketRequestValidatorTests
{
    private readonly CreateTicketRequestValidator _validator = new();

    [Fact]
    public async Task Validate_ValidRequest_Passes()
    {
        var request = new CreateTicketRequest
        {
            Title = "Valid title",
            Description = "Valid description",
            Priority = "High",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyTitle_Fails()
    {
        var request = new CreateTicketRequest
        {
            Title = "",
            Description = "Description",
            Priority = "Medium",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_TitleExceedsMaxLength_Fails()
    {
        var request = new CreateTicketRequest
        {
            Title = new string('A', 201),
            Description = "Description",
            Priority = "Medium",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_InvalidPriority_Fails()
    {
        var request = new CreateTicketRequest
        {
            Title = "Title",
            Description = "Description",
            Priority = "Urgent",
            AssigneeId = 1
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Priority");
    }

    [Fact]
    public async Task Validate_InvalidAssigneeId_Fails()
    {
        var request = new CreateTicketRequest
        {
            Title = "Title",
            Description = "Description",
            Priority = "Low",
            AssigneeId = 0
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AssigneeId");
    }
}

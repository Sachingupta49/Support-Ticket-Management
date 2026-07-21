using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Validators;

namespace SupportTicket.UnitTests.Validators;

public class UpdateTicketStatusRequestValidatorTests
{
    private readonly UpdateTicketStatusRequestValidator _validator = new();

    [Theory]
    [InlineData("Open")]
    [InlineData("InProgress")]
    [InlineData("Resolved")]
    [InlineData("Closed")]
    [InlineData("Cancelled")]
    public async Task Validate_ValidStatus_Passes(string status)
    {
        var request = new UpdateTicketStatusRequest { Status = status };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyStatus_Fails()
    {
        var request = new UpdateTicketStatusRequest { Status = "" };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Status");
    }

    [Fact]
    public async Task Validate_InvalidStatus_Fails()
    {
        var request = new UpdateTicketStatusRequest { Status = "Invalid" };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Status");
    }
}

using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.Application.Validators;

namespace SupportTicket.UnitTests.Validators;

public class CreateCommentRequestValidatorTests
{
    private readonly CreateCommentRequestValidator _validator = new();

    [Fact]
    public async Task Validate_ValidRequest_Passes()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 1,
            Body = "Valid comment"
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyBody_Fails()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 1,
            Body = ""
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Body");
    }

    [Fact]
    public async Task Validate_BodyExceedsMaxLength_Fails()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 1,
            Body = new string('A', 2001)
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Body");
    }
}

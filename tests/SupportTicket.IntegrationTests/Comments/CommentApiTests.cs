using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SupportTicket.Application.DTOs;
using SupportTicket.IntegrationTests.Infrastructure;

namespace SupportTicket.IntegrationTests.Comments;

public class CommentApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public CommentApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetComments_ExistingTicket_ReturnsCommentsOrderedByCreatedAt()
    {
        var response = await _client.GetAsync("/api/tickets/1/comments");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var comments = await response.Content.ReadFromJsonAsync<List<CommentDto>>(JsonOptions);
        comments.Should().NotBeNull();
        comments!.Should().NotBeEmpty();
        comments.Should().BeInAscendingOrder(c => c.CreatedAt);
    }

    [Fact]
    public async Task GetComments_NotFoundTicket_Returns404()
    {
        var response = await _client.GetAsync("/api/tickets/9999/comments");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateComment_ValidRequest_Returns201()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 2,
            Body = "Integration test comment"
        };

        var response = await _client.PostAsJsonAsync("/api/tickets/1/comments", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var comment = await response.Content.ReadFromJsonAsync<CommentDto>(JsonOptions);
        comment.Should().NotBeNull();
        comment!.Body.Should().Be("Integration test comment");
        comment.AuthorName.Should().Be("Bob Agent");
    }

    [Fact]
    public async Task CreateComment_EmptyBody_Returns400()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 2,
            Body = ""
        };

        var response = await _client.PostAsJsonAsync("/api/tickets/1/comments", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Body");
    }

    [Fact]
    public async Task CreateComment_NotFoundTicket_Returns404()
    {
        var request = new CreateCommentRequest
        {
            AuthorId = 2,
            Body = "Comment on missing ticket"
        };

        var response = await _client.PostAsJsonAsync("/api/tickets/9999/comments", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

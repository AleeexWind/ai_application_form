using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ApplicationForm.Api.Auth;
using ApplicationForm.Api.Questionnaires;
using ApplicationForm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationForm.Api.Tests.Questionnaires;

/// <summary>
/// Integration tests for the responses submission endpoint.
/// </summary>
public class ResponsesEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public ResponsesEndpointTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SubmitResponses_WhenAuthenticated_SavesResponsesAndReturnsSuccess()
    {
        var client = _factory.CreateClient();
        await AuthenticateAsync(client);

        var submitRequest = new SubmitResponsesRequest(
        [
            new ResponseAnswerDto(1, "John Doe"),
            new ResponseAnswerDto(2, "30"),
            new ResponseAnswerDto(3, "Developer")
        ]);

        var response = await client.PostAsJsonAsync("/api/responses", submitRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SubmitResponsesResult>();
        Assert.NotNull(result);
        Assert.Equal("Success", result.Message);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var savedResponses = await context.QuestionResponses
            .Include(r => r.Question)
            .OrderBy(r => r.Question!.Position)
            .ToListAsync();

        Assert.Equal(3, savedResponses.Count);
        Assert.Equal("John Doe", savedResponses[0].Answer);
        Assert.Equal("30", savedResponses[1].Answer);
        Assert.Equal("Developer", savedResponses[2].Answer);
    }

    [Fact]
    public async Task SubmitResponses_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();
        var submitRequest = new SubmitResponsesRequest([new ResponseAnswerDto(1, "John Doe")]);

        var response = await client.PostAsJsonAsync("/api/responses", submitRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static async Task AuthenticateAsync(HttpClient client)
    {
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest("user", "password"));
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResult);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
    }
}

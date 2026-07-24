using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ApplicationForm.Api.Auth;
using ApplicationForm.Api.Questionnaires;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApplicationForm.Api.Tests.Questionnaires;

/// <summary>
/// Integration tests for the questions endpoint.
/// </summary>
public class QuestionsEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public QuestionsEndpointTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetQuestions_WhenAuthenticated_ReturnsThreeQuestions()
    {
        var client = _factory.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest("user", "password"));
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginResult);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

        var response = await client.GetAsync("/api/questions");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var questions = await response.Content.ReadFromJsonAsync<List<QuestionDto>>();
        Assert.NotNull(questions);
        Assert.Equal(3, questions.Count);
        Assert.Equal("What is your name?", questions[0].Text);
        Assert.Equal("How old are you?", questions[1].Text);
        Assert.Equal("What is your job?", questions[2].Text);
    }

    [Fact]
    public async Task GetQuestions_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/questions");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

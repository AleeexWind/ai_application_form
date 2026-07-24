using System.Security.Claims;
using ApplicationForm.Domain.Questionnaires;
using ApplicationForm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationForm.Api.Questionnaires;

/// <summary>
/// Handles submission of questionnaire responses.
/// </summary>
[ApiController]
[Route("api/responses")]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ResponsesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Saves submitted answers for the authenticated user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SubmitResponsesResult>> SubmitResponses(
        [FromBody] SubmitResponsesRequest request)
    {
        if (request.Responses.Count == 0)
        {
            return BadRequest(new { message = "At least one response is required." });
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var questions = await _context.Questions.ToListAsync();
        var questionResponses = new List<QuestionResponse>();

        foreach (var item in request.Responses)
        {
            var question = questions.FirstOrDefault(q => q.Position == item.Position);
            if (question is null)
            {
                return BadRequest(new { message = $"Question at position {item.Position} was not found." });
            }

            questionResponses.Add(new QuestionResponse
            {
                UserId = userId,
                QuestionId = question.Id,
                Answer = item.Answer
            });
        }

        _context.QuestionResponses.AddRange(questionResponses);
        await _context.SaveChangesAsync();

        return Ok(new SubmitResponsesResult("Success"));
    }
}

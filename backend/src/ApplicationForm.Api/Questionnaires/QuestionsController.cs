using ApplicationForm.Api.Questionnaires;
using ApplicationForm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationForm.Api.Questionnaires;

/// <summary>
/// Provides questionnaire questions to authenticated users.
/// </summary>
[ApiController]
[Route("api/questions")]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuestionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all questions for the default questionnaire ordered by position.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuestionDto>>> GetQuestions()
    {
        var questions = await _context.Questions
            .OrderBy(q => q.Position)
            .Select(q => new QuestionDto(q.Position, q.Text))
            .ToListAsync();

        return Ok(questions);
    }
}

using ApplicationForm.Domain.Users;

namespace ApplicationForm.Domain.Questionnaires;

/// <summary>
/// Represents a user's answer to a questionnaire question.
/// </summary>
public class QuestionResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuestionId { get; set; }

    public string Answer { get; set; } = string.Empty;

    public User? User { get; set; }

    public Question? Question { get; set; }
}

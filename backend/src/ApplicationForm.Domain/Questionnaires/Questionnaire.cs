namespace ApplicationForm.Domain.Questionnaires;

/// <summary>
/// Represents a questionnaire containing ordered questions.
/// </summary>
public class Questionnaire
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

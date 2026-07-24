namespace ApplicationForm.Domain.Questionnaires;

/// <summary>
/// Represents a single question in a questionnaire.
/// </summary>
public class Question
{
    public int Id { get; set; }

    public int QuestionnaireId { get; set; }

    public int Position { get; set; }

    public string Text { get; set; } = string.Empty;

    public Questionnaire? Questionnaire { get; set; }
}

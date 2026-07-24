namespace ApplicationForm.Api.Questionnaires;

/// <summary>
/// Question data returned to the client.
/// </summary>
public record QuestionDto(int Position, string Text);

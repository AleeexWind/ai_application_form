namespace ApplicationForm.Api.Questionnaires;

/// <summary>
/// A single answer submitted for a question.
/// </summary>
public record ResponseAnswerDto(int Position, string Answer);

/// <summary>
/// Request payload for submitting questionnaire responses.
/// </summary>
public record SubmitResponsesRequest(IReadOnlyList<ResponseAnswerDto> Responses);

/// <summary>
/// Response returned after successful submission.
/// </summary>
public record SubmitResponsesResult(string Message);

namespace ApplicationForm.Domain.Users;

/// <summary>
/// Represents an application user who can access questionnaires.
/// </summary>
public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}

namespace ApplicationForm.Api.Auth;

/// <summary>
/// Login request payload.
/// </summary>
public record LoginRequest(string Username, string Password);

/// <summary>
/// Login response containing the JWT access token.
/// </summary>
public record LoginResponse(string Token);

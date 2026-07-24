using ApplicationForm.Domain.Questionnaires;
using ApplicationForm.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ApplicationForm.Infrastructure.Persistence;

/// <summary>
/// Seeds the in-memory database with default user and questionnaire data.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Users.AnyAsync())
        {
            return;
        }

        context.Users.Add(new User
        {
            Username = "user",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
        });

        var questionnaire = new Questionnaire
        {
            Title = "Application Form",
            Questions =
            [
                new Question { Position = 1, Text = "What is your name?" },
                new Question { Position = 2, Text = "How old are you?" },
                new Question { Position = 3, Text = "What is your job?" }
            ]
        };

        context.Questionnaires.Add(questionnaire);
        await context.SaveChangesAsync();
    }
}

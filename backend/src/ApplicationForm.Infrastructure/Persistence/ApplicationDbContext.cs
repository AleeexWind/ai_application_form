using ApplicationForm.Domain.Questionnaires;
using ApplicationForm.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ApplicationForm.Infrastructure.Persistence;

/// <summary>
/// In-memory database context for the application form.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<QuestionResponse> QuestionResponses => Set<QuestionResponse>();
}

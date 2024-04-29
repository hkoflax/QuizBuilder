using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizBuilder.Domain;

namespace Quiz_Builder_Persistence
{
    public class DataContext: IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public DbSet<SelectedResponse> SelectedResponses { get; set; }
    }
}

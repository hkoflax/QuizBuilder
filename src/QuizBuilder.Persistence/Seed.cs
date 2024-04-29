using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Domain;

namespace QuizBuilder.Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var users = new List<AppUser> {
                    new AppUser {Email="user1@example.com",UserName = "string1", DisplayName = "string1"},
                    new AppUser {Email="user2@example.com",UserName = "user2", DisplayName = "User2"},
                    new AppUser {Email="user3@example.com",UserName = "user3", DisplayName = "User3"}
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Test@123");
                }
            }

            var user1 = await userManager.FindByEmailAsync("user1@example.com");
            var user2 = await userManager.FindByEmailAsync("user2@example.com");

            if (!context.Quizzes.Any())
            {
                var answers1 = new List<Answer>
                {
                    new Answer { IsCorrect = true, Text = "2" },
                    new Answer { IsCorrect = false, Text = "3" },
                    new Answer { IsCorrect = false, Text = "4" }
                };

                var answers2 = new List<Answer>
                {
                    new Answer { IsCorrect = true, Text = "Kelvin" },
                    new Answer { IsCorrect = true, Text = "Fahrenheit" },
                    new Answer { IsCorrect = false, Text = "Gram" },
                    new Answer { IsCorrect = true, Text = "Celsius" },
                    new Answer { IsCorrect = false, Text = "Liters" }
                };

                var question1 = new Question { IsMultipleChoice = false, Text = "What is 1+1", Answers = answers1 };
                var question2 = new Question { IsMultipleChoice = true, Text = "Temperature can be measured in", Answers = answers2 };

                var quiz = new Quiz
                {
                    IsPublished = false,
                    Title = "Quiz 1",
                    Questions = new List<Question> { question1, question2},
                    Author = user1
                };

                await context.Quizzes.AddAsync(quiz);
                await context.SaveChangesAsync();
            }

            var quizz = context.Quizzes
                                   .Include(x => x.Questions)
                                   .ThenInclude(q => q.Answers)
                                   .Where(x => x.Author == user1).FirstOrDefault();

            if (!context.QuizSubmissions.Any())
            {
                var newSubmission = new QuizSubmission()
                {
                    Submittedby = user2,
                    Quiz = quizz,
                    QuizTitle = quizz.Title,
                    CreatedAt = DateTime.Now
                };

                await context.QuizSubmissions.AddAsync(newSubmission);
                await context.SaveChangesAsync();
            };

            var submision = context.QuizSubmissions.FirstOrDefault();

            if (!context.SelectedResponses.Any())
            {
                var responses = new List<SelectedResponse>();

                foreach (var q in quizz.Questions)
                {
                    var selection = new SelectedResponse()
                    {
                        QuizSubmission = submision,
                        Question = q,
                        Answer = q.Answers.First()
                    };
                    responses.Add(selection);
                }

                submision.SelectedResponses = responses;
                submision.IsSubmitted = true;
                submision.SubmittedAt = DateTime.Now;

                context.QuizSubmissions.Update(submision);
                await context.SaveChangesAsync();
            }
        }
    }
}

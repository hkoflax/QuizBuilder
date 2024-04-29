using FluentAssertions;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Core.Test.MockData;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class CreateQuizRequestHandlerTests : HandlerTestsBase
    {
        public CreateQuizRequestHandlerTests(TestSetup testSetup):base(testSetup)
        {

        }


        [Fact]
        public async Task CreateQuiz_HandleAsync_Success()
        {
            // Arrange
            var handler = new CreateQuizRequestHandler(Context, Mapper);
            
            var quiz = MockDataGenerator.GenerateQuiz(DefaultUser, 5, 3);
            var request = Commands.CreateQuiz(quiz, DefaultUser.Id, 5, 3);

            //// Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);
            result.Data.Id.Should().NotBeEmpty();
            result.Data.Author.Id.Should().Be(DefaultUser.Id);
            result.Data.IsPublished.Should().Be(false);
            result.Data.Title.Should().Be(quiz.Title);
            result.Data.Questions.Count.Should().Be(quiz.Questions.Count);

            foreach (var question in result.Data.Questions)
            {
                question.Answers.Should().NotBeNull();
                question.Text.Should().NotBeEmpty();
                question.Id.Should().NotBeEmpty();

                if (question.IsMultipleChoice) question.Answers.Where(x => x.IsCorrect).Count().Should().BeGreaterThan(1);
                else question.Answers.Where(x => x.IsCorrect).Count().Should().Be(1);
                
                foreach (var answer in question.Answers)
                {
                    answer.Text.Should().NotBeEmpty();
                    answer.Id.Should().NotBeEmpty();
                }
            }
        }

        [Fact]
        public async Task CreateQuiz_HandleAsync_Failure()
        {
            // Arrange
            var handler = new CreateQuizRequestHandler(Context, Mapper);

            var userInfo = GetRandomUser();

            var quiz = MockDataGenerator.GenerateQuiz(userInfo, 5, 3);
            var request = Commands.CreateQuiz(quiz, userInfo.Id, 5, 3);

            //// Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

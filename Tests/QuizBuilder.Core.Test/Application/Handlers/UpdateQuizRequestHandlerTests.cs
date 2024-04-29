using FluentAssertions;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Core.Test.MockData;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class UpdateQuizRequestHandlerTests : HandlerTestsBase
    {
        public UpdateQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task UpdateQuiz_HandleAsync_Success()
        {
            // Arrange
            var newTitle = "My Updated Quiz";
            var newQuiz = await CreateANewEmptyQuizAsync();
            var updatedQuiz = new QuizDto(MockDataGenerator.GenerateQuestions(1, 5))
            {
                Title = newTitle
            };

            var request = Commands.UpdateQuiz(updatedQuiz, DefaultUser.Id, newQuiz.Id, 1, 5);

            //// Act
            var handler = new UpdateQuizRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            result.Data.Should().NotBeNull();
            result.Data.Title.Should().Be(newTitle);
            result.Data.Questions.Count.Should().Be(1);
            result.Data.Questions.First().Answers.Count.Should().Be(5);
        }

        [Fact]
        public async Task UpdateQuiz_HandleAsync_Failure()
        {
            // Arrange
            var newTitle = "My Updated Quiz";
            var newQuiz = await CreateANewEmptyQuizAsync();
            var updatedQuiz = new QuizDto(MockDataGenerator.GenerateQuestions(1, 5))
            {
                Title = newTitle
            };

            var request = Commands.UpdateQuiz(updatedQuiz, GetRandomUser().Id, newQuiz.Id, 1, 3);

            //// Act
            var handler = new UpdateQuizRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

using FluentAssertions;
using QuizBuilder.Core.Application.Question.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Core.Test.MockData;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class UpdateQuestionRequestHandlerTests : HandlerTestsBase
    {
        public UpdateQuestionRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task UpdateQuestion_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var newQuestion = MockDataGenerator.GenerateQuestions(1, 5).First();

            var request = Commands.UpdateQuestion(newQuestion, DefaultUser.Id, newQuiz.Questions.First().Id, 5);

            //// Act
            var handler = new UpdateQuestionRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);
            var quiz = RetrieveQuiz(newQuiz.Id);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            result.Data.Should().NotBeNull();
            result.Data.Text.Should().Be(newQuestion.Text);
            result.Data.IsMultipleChoice.Should().Be(newQuestion.IsMultipleChoice);
            result.Data.Answers.Count.Should().Be(newQuestion.Answers.Count);

            quiz.Questions.First().Id.Should().Be(result.Data.Id);
        }

        [Fact]
        public async Task UpdateQuestion_HandleAsync_Failure()
        {
            // Arrange
            _ = await CreateANewQuizAsync(1);
            var newQuestion = MockDataGenerator.GenerateQuestions(1, 5).First();

            var request = Commands.UpdateQuestion(newQuestion, DefaultUser.Id, newQuestion.Id, 5);

            //// Act
            var handler = new UpdateQuestionRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

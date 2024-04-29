using FluentAssertions;
using QuizBuilder.Core.Application.Question.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class DeleteQuestionRequestHandlerTests : HandlerTestsBase
    {
        public DeleteQuestionRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task DeleteQuestion_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);

            var questionToDeleteId = newQuiz.Questions.First().Id;

            //// Act
            var request = Commands.DeleteQuestion(DefaultUser.Id, questionToDeleteId);
            var handler = new DeleteQuestionRequestHandler(Context);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            var data = RetrieveQuiz(newQuiz.Id);
            
            data.Should().NotBeNull();
            data.Questions.Count.Should().Be(0);
        }

        [Fact]
        public async Task DeleteQuestion_HandleAsync_Failure()
        {
            // Arrange
            var userInfo = GetRandomUser();

            var newQuiz = await CreateANewQuizAsync(1);

            var questionToDeleteId = newQuiz.Questions.First().Id;

            //// Act
            var request = Commands.DeleteQuestion(userInfo.Id, questionToDeleteId);
            var handler = new DeleteQuestionRequestHandler(Context);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

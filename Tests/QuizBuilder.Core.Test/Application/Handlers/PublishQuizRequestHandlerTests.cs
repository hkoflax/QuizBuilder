using FluentAssertions;
using Moq;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class PublishQuizRequestHandlerTests : HandlerTestsBase
    {
        public PublishQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task PublishQuiz_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var service = new Mock<IQuizCacheService>();
            var request = Commands.PublishQuiz(DefaultUser.Id, newQuiz.Id, 1);

            //// Act
            var handler = new PublishQuizRequestHandler(Context, service.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            var data = RetrieveQuiz(newQuiz.Id);
            data.Should().NotBeNull();
            data.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task PublishQuiz_HandleAsync_Failure()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var service = new Mock<IQuizCacheService>();
            var request = Commands.PublishQuiz(DefaultUser.Id, newQuiz.Id, 2);

            //// Act
            var handler = new PublishQuizRequestHandler(Context, service.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
            result.Context.Exception.Should().NotBeNull();
            result.Context.Exception.Message.Should().Contain("Cannot publish a quizz");
        }
    }
}

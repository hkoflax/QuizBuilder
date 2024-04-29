using FluentAssertions;
using Moq;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class StartQuizRequestHandlerTests : HandlerTestsBase
    {
        public StartQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task StartQuiz_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var request = Commands.StartQuiz(DefaultUser.Id, newQuiz.Id);

            var service = new Mock<IQuizCacheService>();
            var publisRequest = Commands.PublishQuiz(DefaultUser.Id, newQuiz.Id, 1);
            var publishHandler = new PublishQuizRequestHandler(Context, service.Object);
            _ = await publishHandler.Handle(publisRequest, CancellationToken.None);

            //// Act
            var handler = new StartQuizRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            result.Data.Should().NotBeNull();
            result.Data.SubmissionId.Should().NotBeEmpty();
            result.Data.IsSubmitted.Should().Be(false);
            result.Data.SelectedResponses.Count.Should().Be(0);
        }

        [Fact]
        public async Task StartQuiz_HandleAsync_Failure()
        {
            // Arrange
            var newId = Guid.NewGuid();
            var request = Commands.StartQuiz(DefaultUser.Id, newId);

            //// Act
            var handler = new StartQuizRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }

        [Fact]
        public async Task StartQuiz_HandleAsync_Failure_When_Quiz_Is_Unpublished()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);

            var request = Commands.StartQuiz(DefaultUser.Id, newQuiz.Id);

            //// Act
            var handler = new StartQuizRequestHandler(Context, Mapper);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

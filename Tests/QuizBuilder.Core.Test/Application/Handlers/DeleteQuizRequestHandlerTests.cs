using FluentAssertions;
using Moq;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class DeleteQuizRequestHandlerTests : HandlerTestsBase
    {
        public DeleteQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task DeleteQuiz_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var service = new Mock<IQuizCacheService>();
            service.Setup(x => x.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(RetrieveQuiz(newQuiz.Id));
            var request = Commands.DeleteQuiz(DefaultUser.Id, newQuiz.Id);


            //// Act
            var handler = new DeleteQuizRequestHandler(Context, service.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            var data = RetrieveQuiz(newQuiz.Id);

            data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteQuiz_HandleAsync_Failure()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);
            var service = new Mock<IQuizCacheService>();
            service.Setup(x => x.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(RetrieveQuiz(newQuiz.Id));
            var request = Commands.DeleteQuiz(GetRandomUser().Id, newQuiz.Id);

            //// Act
            var handler = new DeleteQuizRequestHandler(Context, service.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

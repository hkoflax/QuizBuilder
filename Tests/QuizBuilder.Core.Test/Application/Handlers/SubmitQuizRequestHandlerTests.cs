using FluentAssertions;
using Moq;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class SubmitQuizRequestHandlerTests : HandlerTestsBase
    {
        public SubmitQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task SubmitQuiz_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);

            var service = new Mock<IQuizCacheService>();
            service.Setup(x => x.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(RetrieveQuiz(newQuiz.Id));


            var publisRequest = Commands.PublishQuiz(DefaultUser.Id, newQuiz.Id, 1);
            var publishHandler = new PublishQuizRequestHandler(Context, service.Object);
            _ = await publishHandler.Handle(publisRequest, CancellationToken.None);

            var startRequest = Commands.StartQuiz(DefaultUser.Id, newQuiz.Id);
            var startHandler = new StartQuizRequestHandler(Context, Mapper);
            var start = await startHandler.Handle(startRequest, CancellationToken.None);
            var submissionId = start.Data.SubmissionId;
            var submission = new CreateSubmitDto()
            {
                QuizId = newQuiz.Id,
                SelectedResponses = new List<CreateSelectedResponseDto>
                {
                    new()
                    {
                        QuestionId= newQuiz.Questions.First().Id,
                        AnswerId = newQuiz.Questions.First().Answers.First().Id
                    }
                }
            };

            var submitQuizRequest = Commands.SubmitQuiz(DefaultUser.Id, submissionId, submission);
            var handler = new SubmitQuizRequestHandler(Context, Mapper, service.Object);
            //// Act

            var result = await handler.Handle(submitQuizRequest, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);

            result.Data.Should().NotBeNull();
            result.Data.FinalScore.Should().NotBe(0);
            result.Data.IsSubmitted.Should().Be(true);
        }

        [Fact]
        public async Task SubmitQuiz_HandleAsync_Failure()
        {
            // Arrange
            var newQuiz = await CreateANewQuizAsync(1);

            var service = new Mock<IQuizCacheService>();
            service.Setup(x => x.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(RetrieveQuiz(newQuiz.Id));


            var publisRequest = Commands.PublishQuiz(DefaultUser.Id, newQuiz.Id, 1);
            var publishHandler = new PublishQuizRequestHandler(Context, service.Object);
            _ = await publishHandler.Handle(publisRequest, CancellationToken.None);

            var startRequest = Commands.StartQuiz(DefaultUser.Id, newQuiz.Id);
            var startHandler = new StartQuizRequestHandler(Context, Mapper);
            var start = await startHandler.Handle(startRequest, CancellationToken.None);
            var submissionId = start.Data.SubmissionId;
            var submission = new CreateSubmitDto()
            {
                QuizId = newQuiz.Id,
                SelectedResponses = new List<CreateSelectedResponseDto>
                {
                    new()
                    {
                        QuestionId= Guid.NewGuid(),
                        AnswerId = newQuiz.Questions.First().Answers.First().Id
                    }
                }
            };

            var submitQuizRequest = Commands.SubmitQuiz(DefaultUser.Id, submissionId, submission);
            var handler = new SubmitQuizRequestHandler(Context, Mapper, service.Object);
            //// Act

            var result = await handler.Handle(submitQuizRequest, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
            result.Context.Exception.Should().NotBeNull();
            result.Context.Exception.Message.Should().Contain("does not belong to this quiz");
        }
    }
}

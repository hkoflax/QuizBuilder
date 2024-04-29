using FluentAssertions;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.Abstractions;
using QuizBuilder.Core.Test.MockData;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Application.Handlers
{
    public class AddQuestionToQuizRequestHandlerTests : HandlerTestsBase
    {
        public AddQuestionToQuizRequestHandlerTests(TestSetup testSetup) : base(testSetup)
        {
        }


        [Fact]
        public async Task AddQuestionToQuiz_HandleAsync_Success()
        {
            // Arrange
            var newQuiz = await CreateANewEmptyQuizAsync();

            var handler = new AddQuestionToQuizRequestHandler(Context, Mapper);
            var question = MockDataGenerator.GenerateQuestions(1, 5).FirstOrDefault();
            
            //// Act
            var request = Commands.AddQuestionToQuiz(question, DefaultUser.Id, newQuiz.Id, 5);
            var result = await handler.Handle(request, CancellationToken.None);


            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(true);
            result.Data.Id.Should().Be(newQuiz.Id);
            result.Data.Author.Id.Should().Be(DefaultUser.Id);
            result.Data.IsPublished.Should().Be(false);
            result.Data.Title.Should().Be(newQuiz.Title);
            result.Data.Questions.Count.Should().Be(1);

            foreach (var ques in result.Data.Questions)
            {
                ques.Answers.Should().NotBeNull();
                ques.Answers.Count.Should().Be(question.Answers.Count);
                ques.Text.Should().Be(question.Text);
            }
        }

        [Fact]
        public async Task AddQuestionToQuiz_HandleAsync_Failure_When_UserId_Not_Exist()
        {
            // Arrange
            var userInfo = GetRandomUser();

            var newQuiz = await CreateANewEmptyQuizAsync();

            var handler = new AddQuestionToQuizRequestHandler(Context, Mapper);
            var question = MockDataGenerator.GenerateQuestions(1, 5).FirstOrDefault();

            //// Act
            var request = Commands.AddQuestionToQuiz(question, userInfo.Id, newQuiz.Id, 5);
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBe(null);
            result.Succeeded.Should().Be(false);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Account;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Quiz.Handlers.Commands;
using QuizBuilder.Core.Application.Requests;
using QuizBuilder.Core.Test.MockData;
using QuizBuilder.Domain;
using QuizBuilder.Domain.Test;

namespace QuizBuilder.Core.Test.Abstractions
{
    public abstract class HandlerTestsBase : IClassFixture<TestSetup>
    {
        protected readonly IMapper Mapper;
        protected readonly DataContext Context;
        protected UserInfoDto DefaultUser => GetDefaultUserAsync().Result;
        public HandlerTestsBase(TestSetup testSetup)
        {
            Mapper = testSetup.Mapper;
            Context = testSetup.DataContext;
        }

        protected async Task<UserInfoDto> GetDefaultUserAsync()
        {
            return await Context.Users.Select(x => new UserInfoDto { Id = x.Id, DisplayName = x.DisplayName, UserName = x.UserName }).FirstAsync();
        }

        protected async Task<QuizDto> CreateANewQuizAsync(int questionCount = 5, QuizDto quizDto = null, UserInfoDto userInfoDto = null)
        {
            quizDto = quizDto ?? MockDataGenerator.GenerateQuiz(userInfoDto ?? DefaultUser, questionCount, 3);
            return await DoCreateQuiz(quizDto);
        }

        protected async Task<QuizDto> CreateANewEmptyQuizAsync(UserInfoDto userInfoDto = null)
        {
            var quizDto = MockDataGenerator.GenerateQuizWithNoQuestions(userInfoDto ?? DefaultUser);
            return await DoCreateQuiz(quizDto);
        }

        protected Quiz RetrieveQuiz(Guid quizId)
        {
            return Context.Quizzes.Where(x => x.Id == quizId).FirstOrDefault();
        }

        protected Question RetrieveQuestion(Guid questionId)
        {
            return Context.Questions.Where(x => x.Id == questionId).FirstOrDefault();
        }

        protected UserInfoDto GetRandomUser()
        {
            var newId = Guid.NewGuid();
            return new UserInfoDto() { Id = newId.ToString(), DisplayName = "userX", UserName = "UserX" };
        }

        private async Task<QuizDto> DoCreateQuiz(QuizDto quizDto)
        {
            var handler = new CreateQuizRequestHandler(Context, Mapper);
            var request = Commands.CreateQuiz(quizDto, DefaultUser.Id, 5, 3);
            var result = await handler.Handle(request, CancellationToken.None);
            return result.Data;
        }
    }
}

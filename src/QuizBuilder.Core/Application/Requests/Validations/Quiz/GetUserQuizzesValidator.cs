using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class GetUserQuizzesValidator : AbstractValidator<GetUserQuizzes>
    {
        public GetUserQuizzesValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();
        }
    }
}

using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class GetMySubmissionsValidator : AbstractValidator<GetMySubmissions>
    {
        public GetMySubmissionsValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();
        }
    }
}

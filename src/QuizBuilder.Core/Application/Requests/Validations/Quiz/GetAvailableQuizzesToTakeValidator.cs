using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class GetAvailableQuizzesToTakeValidator : AbstractValidator<GetAvailableQuizzesToTake>
    {
        public GetAvailableQuizzesToTakeValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("The 'PageSize' field must be greater than 1");

            RuleFor(c => c.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("The 'PageNumber' field must be greater than 1");
        }
    }
}

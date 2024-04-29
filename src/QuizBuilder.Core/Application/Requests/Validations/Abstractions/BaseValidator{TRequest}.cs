using FluentValidation;

namespace QuizBuilder.Core.Application.Requests.Validations.Abstractions
{
    public abstract class BaseValidator<TRequest>: AbstractValidator<TRequest>
    {
        protected static bool IsCountInRange(int count, int maxAnswerOptionsPerQuestion)
        {
            return count >= 2 && count <= maxAnswerOptionsPerQuestion;
        }
    }
}

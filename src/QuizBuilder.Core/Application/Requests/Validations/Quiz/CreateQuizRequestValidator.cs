using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class CreateQuizRequestValidator : AbstractValidator<CreateQuizRequest>
    {
        public CreateQuizRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Details)
                .SetValidator(c => new QuizzDtoValidator(c.MaxAllowedQuestionsPerQuiz, c.MaxAnswerOptionsPerQuestion));
        }
    }
}

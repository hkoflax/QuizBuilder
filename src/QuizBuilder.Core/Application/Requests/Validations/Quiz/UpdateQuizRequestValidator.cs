using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class UpdateQuizRequestValidator : AbstractValidator<UpdateQuizRequest>
    {
        public UpdateQuizRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.QuizId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Details)
                .SetValidator(c => new QuizzDtoValidator(c.MaxAllowedQuestionsPerQuiz, c.MaxAnswerOptionsPerQuestion));
        }
    }
}

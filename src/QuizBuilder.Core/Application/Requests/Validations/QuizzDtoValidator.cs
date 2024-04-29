using FluentValidation;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests.Validations.Abstractions;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class QuizzDtoValidator : BaseValidator<QuizDto>
    {
        public QuizzDtoValidator(int maxAllowedQuestionsPerQuiz = default, int maxAnswerOptionsPerQuestion = default)
        {
            RuleFor(c => c)
                .NotNull()
                .WithMessage("Cannot a create an empty quiz");

            RuleFor(c => c.Title).NotEmpty().NotNull();

            RuleFor(c => c.Questions.Count())
                .LessThanOrEqualTo(maxAllowedQuestionsPerQuiz)
                .WithMessage($"Quizz Should not exceed {maxAllowedQuestionsPerQuiz} questions.");

            RuleForEach(q => q.Questions)
                .ChildRules(question =>
                {
                    question.RuleFor(c => c)
                            .SetValidator(c => new QuestionDtoValidator(maxAnswerOptionsPerQuestion));
                });
        }
    }
}

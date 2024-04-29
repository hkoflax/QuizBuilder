using FluentValidation;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests.Validations.Abstractions;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class QuestionDtoValidator : BaseValidator<QuestionDto>
    {
        public QuestionDtoValidator(int maxAnswerOptionsPerQuestion = default)
        {
            RuleFor(c => c)
                .NotNull()
                .WithMessage("Cannot add an empty question.");

            RuleFor(c => c.Text).NotNull().NotEmpty()
                            .WithMessage("Question text is required.");

            RuleFor(c => c.Answers)
                 .Must((c, answers) => IsCountInRange(answers.Count, maxAnswerOptionsPerQuestion))
                 .WithMessage(c => $"Answer options Should be between 2 and {maxAnswerOptionsPerQuestion} possibilities.");

            RuleFor(q => q.IsMultipleChoice)
                            .Custom((isMultipleChoice, context) =>
                            {
                                if (isMultipleChoice)
                                {
                                    int correctAnswersCount = context.InstanceToValidate.Answers.Count(answer => answer.IsCorrect);
                                    if (correctAnswersCount < 2)
                                    {
                                        context.AddFailure("For multiple choice questions, at least 2 answers should be correct.");
                                    }
                                }
                                else
                                {
                                    int correctAnswersCount = context.InstanceToValidate.Answers.Count(answer => answer.IsCorrect);
                                    if (correctAnswersCount != 1)
                                    {
                                        context.AddFailure("For single choice questions, exactly 1 answer should be correct.");
                                    }
                                }
                            });

            RuleForEach(q => q.Answers)
                .ChildRules(answer =>
                {
                    answer.RuleFor(a => a.Text).NotNull().NotEmpty().WithMessage("An answer text is required.");
                });
        }
    }
}

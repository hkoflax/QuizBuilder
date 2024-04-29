using Bogus;
using QuizBuilder.Core.Application.Abstractions.Models.Account;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;

namespace QuizBuilder.Core.Test.MockData
{
    public static class MockDataGenerator
    {
        public static IEnumerable<QuestionDto> GenerateQuestions(int questionCount, int answerPerQuestion)
        {
            var isMultipleChoice = new Faker().Random.Bool();
            var generator = new Faker<QuestionDto>()
                .StrictMode(false)
                .RuleFor(q => q.Answers, f => GenerateAnswers(isMultipleChoice, answerPerQuestion))
                .RuleFor(q => q.IsMultipleChoice, f => isMultipleChoice)
                .RuleFor(q => q.Text, f => f.Lorem.Sentence());

            return generator.Generate(questionCount);
        }

        public static List<AnswerDto> GenerateAnswers(bool isMultipleChoice, int answerPerQuestion)
        {
            var answers = new List<AnswerDto>();

            int correctAnswerCount = isMultipleChoice ? new Faker().Random.Int(2, answerPerQuestion) : 1;

            for (int i = 0; i < answerPerQuestion; i++)
            {
                bool isCorrect = i < correctAnswerCount;
                answers.Add(new AnswerDto { Text = new Faker().Lorem.Word(), IsCorrect = isCorrect });
            }

            return answers;
        }

        public static QuizDto GenerateQuiz(UserInfoDto Author, int questionCount, int answerPerQuestion)
        {
            var generator = new Faker<QuizDto>()
                .StrictMode(false)
                .RuleFor(q => q.Questions, f => GenerateQuestions(questionCount, answerPerQuestion))
                .RuleFor(q => q.Author, f => Author)
                .RuleFor(q => q.Title, f => $"{f.Lorem.Word()} Quiz");

            return generator.Generate();
        }

        public static QuizDto GenerateQuizWithNoQuestions(UserInfoDto Author)
        {
            var generator = new Faker<QuizDto>()
                .StrictMode(false)
                .RuleFor(q => q.Questions, f => Array.Empty<QuestionDto>())
                .RuleFor(q => q.Author, f => Author)
                .RuleFor(q => q.Title, f => $"{f.Lorem.Word()} Quiz");

            return generator.Generate();
        }
    }
}

using QuizBuilder.API.DTOs.Quiz.Abstractions;

namespace QuizBuilder.API.DTOs.Quiz.Create
{
    public class CreateAnswerModel : AnswerModelBase
    {
        public bool IsCorrect { get; set; }
    }
}

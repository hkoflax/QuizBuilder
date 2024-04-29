using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.API.DTOs.Quiz.Abstractions;

namespace QuizBuilder.API.DTOs.Quiz.Create
{
    public class CreateQuestionModel : QuestionModelBase, IApiModel
    {
        public IEnumerable<CreateAnswerModel> Answers { get; set; } = Array.Empty<CreateAnswerModel>();
    }
}

using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.API.DTOs.Quiz.Abstractions;

namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class ListQuestionModel : QuestionModelBase, IApiModel
    {
        public Guid Id { get; set; }
        public IEnumerable<ListAnswerModel> Answers { get; private set; } = Array.Empty<ListAnswerModel>();
    }
}

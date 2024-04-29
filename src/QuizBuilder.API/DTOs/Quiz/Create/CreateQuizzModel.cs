using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.API.DTOs.Quiz.Abstractions;

namespace QuizBuilder.API.DTOs.Quiz.Create
{
    public class CreateQuizzModel: QuizModelBase, IApiModel
    {
        public IEnumerable<CreateQuestionModel> Questions { get; set; } = Array.Empty<CreateQuestionModel>();
    }
}

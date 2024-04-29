using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.API.DTOs.Quiz.Abstractions;

namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class ListQuizModel : QuizModelBase, IApiModel
    {
        public Guid Id { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<ListQuestionModel> Questions { get; private set; } = Array.Empty<ListQuestionModel>();
        public string CreatedBy { get; set; }
        public string PublishedDate { get; set; }

    }
}

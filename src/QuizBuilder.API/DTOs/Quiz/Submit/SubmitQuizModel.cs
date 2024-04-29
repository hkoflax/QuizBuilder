using QuizBuilder.API.Abstractions.Models;

namespace QuizBuilder.API.DTOs.Quiz.Submit
{
    public class SubmitQuizModel: IApiModel
    {
        public Guid QuizId { get; set; }
        public ICollection<SubmitSelectedResponseModel> SelectedResponses { get; set; } = Array.Empty<SubmitSelectedResponseModel>();
    }
}

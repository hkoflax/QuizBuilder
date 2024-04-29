using QuizBuilder.API.Abstractions.Models;

namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class QuizSubmissionListModel : IApiModel
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<SubmissiondModelBase> Submissions { get; set; } = Array.Empty<SubmissiondModelBase>();
    }
}

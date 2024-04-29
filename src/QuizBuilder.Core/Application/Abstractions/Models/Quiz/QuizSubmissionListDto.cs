namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class QuizSubmissionListDto: IApplicationModel
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<SubmissiondModelBaseDto> Submissions { get; set; } = Array.Empty<SubmissiondModelBaseDto>();
    }
}

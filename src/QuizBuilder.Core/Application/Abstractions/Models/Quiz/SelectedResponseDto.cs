namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class SelectedResponseDto: IApplicationModel
    {
        public SelectedResponseDto(IEnumerable<string> answers = null)
        {
            SetAnswers(answers);
        }

        private void SetAnswers(IEnumerable<string> answers)
        {
            Answers = answers?.ToArray() ?? Array.Empty<string>();
        }

        public Guid QuestionId { get; set; }
        public string Question { get; set; }
        public ICollection<string> Answers { get; private set; } = Array.Empty<string>();
    }
}

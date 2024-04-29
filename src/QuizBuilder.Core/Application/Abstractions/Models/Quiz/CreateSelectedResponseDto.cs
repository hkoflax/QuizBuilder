namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class CreateSelectedResponseDto
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
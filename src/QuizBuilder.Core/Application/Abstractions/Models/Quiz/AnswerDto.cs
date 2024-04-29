using Newtonsoft.Json;

namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class AnswerDto : IApplicationModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public double Weight { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}

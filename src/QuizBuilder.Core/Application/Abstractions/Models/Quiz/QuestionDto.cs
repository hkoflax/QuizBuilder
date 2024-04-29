using Newtonsoft.Json;

namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class QuestionDto : IApplicationModel
    {
        public QuestionDto(): this(null)
        {
            
        }
        public QuestionDto(IEnumerable<AnswerDto> answers = null)
        {
            SetAnswers(answers);
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public ICollection<AnswerDto> Answers { get; private set; } = Array.Empty<AnswerDto>();
        public bool IsMultipleChoice { get; set; }

        public void SetAnswers(IEnumerable<AnswerDto> src)
        {
            Answers = src?.ToArray() ?? Array.Empty<AnswerDto>();
        }

        public void SetAnswersWeights()
        {
            var correctAnswers = Answers.Where(x => x.IsCorrect).ToList();
            var weightPerRightAnswer = Math.Round(1.0 / correctAnswers.Count, 2);

            var wrongAnswers = Answers.Where(x => !x.IsCorrect).ToList();
            var weightPerWrongAnswer = -Math.Round(1.0 / wrongAnswers.Count, 2);

            foreach (var answer in Answers)
            {
                answer.Weight = answer.IsCorrect? weightPerRightAnswer: weightPerWrongAnswer;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}

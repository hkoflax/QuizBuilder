using Newtonsoft.Json;
using QuizBuilder.Core.Application.Abstractions.Models.Account;
using Formatting = Newtonsoft.Json.Formatting;

namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class QuizDto : IApplicationModel
    {
        public QuizDto() : this(null)
        {

        }
        public QuizDto(IEnumerable<QuestionDto> questions = null)
        {
            SetQuestions(questions);
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }

        public ICollection<QuestionDto> Questions { get; private set; } = Array.Empty<QuestionDto>();
        public UserInfoDto Author { get; set; }

        private void SetQuestions(IEnumerable<QuestionDto> src)
        {
            Questions = src?.ToArray() ?? Array.Empty<QuestionDto>();
        }

        public void SetAnswersWeights()
        {
            foreach (var question in Questions)
            {
                question.SetAnswersWeights();
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}

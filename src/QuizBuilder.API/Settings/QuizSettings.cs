namespace QuizBuilder.API.Settings
{
    public class QuizSettings
    {
        public const string configurationSection = "QuizConfiguration";

        private int _minAllowedQuestionsPerQuiz;
        private int _maxAnswerOptionsPerQuestion;
        private int _maxAllowedQuestionsPerQuiz;

        public int MinAllowedQuestionsPerQuiz
        {
            get => _minAllowedQuestionsPerQuiz;
            set => _minAllowedQuestionsPerQuiz = value < 1 ? 1 : value;
        }
        public int MaxAllowedQuestionsPerQuiz
        {
            get => _maxAllowedQuestionsPerQuiz;
            set => _maxAllowedQuestionsPerQuiz = value < _minAllowedQuestionsPerQuiz ? _minAllowedQuestionsPerQuiz : value;
        }
        public int MaxAnswerOptionsPerQuestion
        {
            get => _maxAnswerOptionsPerQuestion;
            set => _maxAnswerOptionsPerQuestion = value < 2 ? 2 : value;
        }
    }
}

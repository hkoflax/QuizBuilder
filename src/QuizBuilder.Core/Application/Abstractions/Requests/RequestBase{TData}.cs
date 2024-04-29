namespace QuizBuilder.Core.Application.Abstractions.Requests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBase{TData}"/> class.
    /// </summary>
    public abstract class RequestBase<TData> : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TData}"/> class.
        /// </summary>
        protected RequestBase(): this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBase{TData}"/> class.
        /// </summary>
        /// <param name="userId">The id of the client in the session.</param>
        protected RequestBase(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        ///  Gets the client id.
        /// </summary>
        public string UserId { get; }

        public int MaxAnswerOptionsPerQuestion { get; set; }
        public int MaxAllowedQuestionsPerQuiz { get; set; }
        public int MinAllowedQuestionsPerQuiz { get; set; }
    }
}

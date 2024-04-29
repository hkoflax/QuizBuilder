using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Domain;
using static System.Formats.Asn1.AsnWriter;

namespace QuizBuilder.Core.Application.Quiz.Handlers.Commands
{
    public class SubmitQuizRequestHandler : IRequestHandler<SubmitQuizRequest, Response<SubmitQuizRequest, QuizSubmissionDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public readonly IQuizCacheService _quizCacheService;

        public SubmitQuizRequestHandler(DataContext dataContext, IMapper mapper, IQuizCacheService quizCacheService )
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _quizCacheService = quizCacheService ?? throw new ArgumentNullException(nameof(quizCacheService));
        }

        public async Task<Response<SubmitQuizRequest, QuizSubmissionDto>> Handle(SubmitQuizRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            try
            {
                var submission = await _context.QuizSubmissions
                    .Include(x => x.Submittedby)
                    .Where(x => x.Id == request.SubmissionId).FirstOrDefaultAsync();

                if (submission.IsSubmitted)
                {
                    return request.Faulted<SubmitQuizRequest, QuizSubmissionDto>(new Exception("This submission has been completed"));
                }

                if (submission == null || submission.Submittedby.Id != request.UserId)
                {
                    return request.Failed<SubmitQuizRequest, QuizSubmissionDto>();
                }

                var quiz = await _quizCacheService.GetItemAsync(request.Details.QuizId);

                if (quiz == null) return request.Failed<SubmitQuizRequest, QuizSubmissionDto>();

                submission.SubmittedAt = DateTime.Now;
                var allQuestions = quiz.Questions;

                var groupedResponse = request.Details.SelectedResponses
                                                     .GroupBy(sr => sr.QuestionId)
                                                     .Select(group => new QuestionScore(
                                                        0.0,
                                                        group.Key,
                                                        group.Select(sr => sr.AnswerId).ToList()
                                                     )).ToList();


                var answers = allQuestions.SelectMany(x => x.Answers).ToList();
                var saveList = new List<SelectedResponse>();

                foreach (var response in groupedResponse)
                {
                    var referenceAnswers = allQuestions.Where(x => x.Id == response.QuestionId)
                                                        .Select(x => x.Answers).FirstOrDefault();
                    if (referenceAnswers == null)
                    {
                        return request.Faulted<SubmitQuizRequest, QuizSubmissionDto>(new Exception($"QuestionId {response.QuestionId} does not belong to this quiz."));
                    }

                    double totalPoint = Math.Round(referenceAnswers.Select(a => a.Weight).Sum(), 2);
                    double tempScore = 0;
                    foreach (var answer in response.AnswerIds)
                    {                        
                        if (referenceAnswers.Any(x => x.Id == answer))
                        {
                            var res = new SelectedResponse()
                            {
                                Answer = referenceAnswers.Where(x => x.Id == answer).FirstOrDefault(),
                                Question = allQuestions.Where(x => x.Id == response.QuestionId).FirstOrDefault(),
                            };
                            saveList.Add(res);
                            tempScore += res.Answer.Weight;
                        }
                    }

                    if (tempScore == totalPoint) tempScore = 0;
                    if (tempScore > 0 && Math.Round(tempScore - 1.0 ,2) == totalPoint) tempScore = 1;
                    if (tempScore < 0 && Math.Round(tempScore + 1.0 ,2) == totalPoint) tempScore = -1;

                    response.Score = tempScore;
                }

                var TotalScore = groupedResponse.Select(x => x.Score).Sum();

                submission.FinalScore = Math.Round(TotalScore, 2);
                submission.QuizTitle = quiz.Title;
                submission.IsSubmitted = true;

                submission.SelectedResponses = saveList;
                submission.Quiz = quiz;

                _context.Entry(quiz).State = EntityState.Unchanged;
                _context.Entry(submission.Submittedby).State = EntityState.Unchanged;

                _context.QuizSubmissions.Update(submission);
                await _context.SaveChangesAsync();
                return request.Completed(_mapper.Map<QuizSubmissionDto>(submission));
            }
            catch (Exception ex)
            {
                return request.Failed<SubmitQuizRequest, QuizSubmissionDto>(ex);
            }
        }
    }

    internal class QuestionScore
    {
        public double Score { get; set; }
        public Guid QuestionId { get; }
        public List<Guid> AnswerIds { get; }

        public QuestionScore(Guid questionId)
        {
            AnswerIds = new List<Guid>();
        }

        public QuestionScore(double score, Guid questionId, List<Guid> answerIds)
        {
            Score = score;
            QuestionId = questionId;
            AnswerIds = answerIds;
        }
    }
}

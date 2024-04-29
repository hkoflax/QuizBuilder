using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Abstractions.Requests;
using QuizBuilder.Core.Application.Requests.Question;

namespace QuizBuilder.Core.Application.Question.Handlers.Commands
{
    public class UpdateQuestionRequestHandler : IRequestHandler<UpdateQuestionRequest, Response<UpdateQuestionRequest, QuestionDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateQuestionRequestHandler(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Response<UpdateQuestionRequest, QuestionDto>> Handle(UpdateQuestionRequest request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            try
            {
                var question = await _context.Questions
                                         .Include(q => q.Answers)
                                         .Include(q => q.Quiz)
                                         .ThenInclude(q => q.Author)
                                         .FirstOrDefaultAsync( q => q.Id == request.QuestionId, cancellationToken)
                                         .ConfigureAwait(false);

                if (question != null && request.UserId == question.Quiz.Author.Id)
                {
                    if (question.Quiz.IsPublished)
                        return request.Faulted<UpdateQuestionRequest, QuestionDto>(new Exception("You cannot update a question that belongs to a published Quiz"));

                    var previousAnswers = question.Answers;

                    request.Details.SetAnswersWeights();
                    var updateQuestion = _mapper.Map<QuizBuilder.Domain.Question>(request.Details);

                    question.Text = updateQuestion.Text;
                    question.Answers = updateQuestion.Answers;
                    question.IsMultipleChoice = updateQuestion.IsMultipleChoice;               

                    _context.Answers.RemoveRange(previousAnswers);
                    _context.Questions.Update(question);
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    return request.Completed(_mapper.Map<QuestionDto>(question));
                }

                return request.Failed<UpdateQuestionRequest, QuestionDto>();
            }
            catch (Exception ex)
            {
                return request.Failed<UpdateQuestionRequest, QuestionDto>(ex);
            }
        }
    }
}
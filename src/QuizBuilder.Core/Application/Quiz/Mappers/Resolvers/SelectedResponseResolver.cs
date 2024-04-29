using AutoMapper;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Domain;

namespace QuizBuilder.Core.Application.Quiz.Mappers.Resolvers
{
    public class SelectedResponseResolver : ITypeConverter<ICollection<SelectedResponse>, ICollection<SelectedResponseDto>>
    {
        public ICollection<SelectedResponseDto> Convert(ICollection<SelectedResponse> source, ICollection<SelectedResponseDto> destination, ResolutionContext context)
        {
            if (source == null) return Array.Empty<SelectedResponseDto>(); 

            var groupedResponses = source.GroupBy(sr => sr.Question.Id)
                                         .Select(group => new SelectedResponseDto(group.Select(sr => sr.Answer.Text).ToList())
                                         {
                                             QuestionId = group.Key,
                                             Question = group.First().Question.Text
                                         })
                                         .ToList();

            return groupedResponses;
        }
    }
}
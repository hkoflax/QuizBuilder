using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.Core.Application.Abstractions.Models;

namespace QuizBuilder.API.Abstractions.Mappings
{
    /// <summary>
    /// Api to Application/Dto model mapping.
    /// </summary>
    /// <typeparam name="TApiModel">The <see cref="IApiModel"/> type.</typeparam>
    /// <typeparam name="TApplicationModel">The <see cref="IApplicationModel"/> type.</typeparam>
    public interface IApiModelMapper<TApiModel, TApplicationModel>
             where TApiModel : class, IApiModel
             where TApplicationModel : class, IApplicationModel
    {
        /// <summary>
        /// Maps to an api model from an application model.
        /// </summary>
        /// <param name="source">The <see cref="IApplicationModel"/> to map.</param>
        /// <returns>The resulting <see cref="IApiModel"/>.</returns>
        TApiModel MapTo(TApplicationModel source);

        /// <summary>
        /// Maps to a collection of api models from a collection of application models.
        /// </summary>
        /// <param name="source">The <see cref="IApplicationModel"/> to map.</param>
        /// <returns>The resulting <see cref="IApiModel"/>.</returns>
        IEnumerable<TApiModel> MapTo(IEnumerable<TApplicationModel> source);

        /// <summary>
        /// Maps from an api model to an application model.
        /// </summary>
        /// <param name="source">The <see cref="IApplicationModel"/> to map.</param>
        /// <returns>The resulting <see cref="IApiModel"/>.</returns>
        TApplicationModel MapFrom(TApiModel source);

        /// <summary>
        /// Maps from a collection of api models to a collection of application models.
        /// </summary>
        /// <param name="source">The <see cref="IApplicationModel"/> to map.</param>
        /// <returns>The resulting <see cref="IApiModel"/>.</returns>
        IEnumerable<TApplicationModel> MapFrom(IEnumerable<TApiModel> source);
    }
}

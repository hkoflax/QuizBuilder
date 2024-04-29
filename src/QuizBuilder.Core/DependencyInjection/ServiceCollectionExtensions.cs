using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizBuilder.Core.Application;
using QuizBuilder.Core.Application.Behaviours;
using QuizBuilder.Core.Application.Quiz.Mappers.Resolvers;
using QuizBuilder.Core.Domain.Abstractions;
using QuizBuilder.Core.Domain.Services;
using System.Reflection;

namespace QuizBuilder.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQuizBuilderCoreApplication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddCachingServices(configuration);

            return services.AddRequestValidators()
                           .AddApplicationBehaviours()
                           .AddMediatRHandlers()
                           .AddModelMapping();
        }

        /// <summary>
        /// Add mapping profiles to the services collection.
        /// </summary>
        /// <param name="services">THe current <see cref="IServiceCollection"/>.</param>
        /// <returns>The updated reference to the <paramref name="services"/> collection.</returns>
        internal static IServiceCollection AddModelMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(opt => opt.AddProfiles(MappingProfiles.All));
            services.AddTransient<SelectedResponseResolver>();

            return services;
        }

        /// <summary>
        /// Adds the MediatR handlers to the services collection.
        /// </summary>
        /// <param name="services">THe current <see cref="IServiceCollection"/>.</param>
        /// <returns>The updated reference to the <paramref name="services"/> collection.</returns>
        internal static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
            => services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        /// <summary>
        /// Adds the Fluent Validators to the services collection.
        /// </summary>
        /// <param name="services">THe current <see cref="IServiceCollection"/>.</param>
        /// <returns>The updated reference to the <paramref name="services"/> collection.</returns>
        internal static IServiceCollection AddRequestValidators(this IServiceCollection services)
            => services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Adds behaviours to the services collection.
        /// </summary>
        /// <param name="services">THe current <see cref="IServiceCollection"/>.</param>
        /// <returns>The updated reference to the <paramref name="services"/> collection.</returns>
        internal static IServiceCollection AddApplicationBehaviours(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }

        internal static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IQuizCacheService, QuizCacheService>();
            return services;
        }
    }
}

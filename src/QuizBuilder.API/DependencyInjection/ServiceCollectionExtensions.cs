using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizBuilder.Services;
using QuizBuilder.Settings;
using Quiz_Builder_Persistence;
using Quiz_Builder_Persistence.DependencyInjection;
using QuizBuilder.API.Abstractions.Mappings;
using QuizBuilder.API.Mappings;
using QuizBuilder.Core.DependencyInjection;
using System.Text;
using Microsoft.OpenApi.Models;
using QuizBuilder.API.Settings;
using QuizBuilder.Core.Domain.Settings;
using QuizBuilder.API.Mappings.Resolvers;

namespace QuizBuilder.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services required by the application.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettingsConfiguration(configuration);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenConfiguration();

            services.AddDBContexts(configuration);
            services.AddQuizBuilderCoreApplication(configuration);
            services.AddApiModelMapping()
                    .AddAutoMapper(opt => opt.AddProfiles(MappingProfiles.ApiProfiles));


            services.AddPersistenceServices(configuration);
            services.AddJWTTokenservices(configuration);
            return services;
        }

        /// <summary>
        /// Adds the DBContext for the application to use.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddDBContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }

        /// <summary>
        /// Adds the configuration settings as IOptions.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenSettings>(configuration.GetSection(TokenSettings.configurationSection));
            services.Configure<QuizSettings>(configuration.GetSection(QuizSettings.configurationSection));
            services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.configurationSection));

            return services;
        }

        /// <summary>
        /// Adds the JWT Tokwn service configuration for authentication in the app.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddJWTTokenservices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new TokenSettings();
            configuration.GetSection(TokenSettings.configurationSection).Bind(settings);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.TokenKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.AddScoped<TokenService>();
            return services;
        }

        /// <summary>
        /// Adds the common API model mapping services, profiles and default value resolvers.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddApiModelMapping(this IServiceCollection services)
        {
            services.AddTransient(typeof(IApiModelMapper<,>), typeof(DefaultApiModelMapper<,>));
            services.AddTransient<SelectedResponseModelResolver>();
            return services;
        }

        /// <summary>
        /// Add swagger Gen configuration in the service collection.
        /// </summary>
        /// <param name="services">The current <see cref="IServiceCollection"/> to add the service registrations to.</param>
        /// <returns>The updated <paramref name="services"/> reference.</returns>
        public static IServiceCollection AddSwaggerGenConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizBuilderApi", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return services;
        }
    }
}

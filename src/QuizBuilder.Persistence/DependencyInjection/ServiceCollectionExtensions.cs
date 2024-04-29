using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizBuilder.Domain;

namespace Quiz_Builder_Persistence.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddIdentityServices();
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DataContext>();
            return services;
        }
    }
}

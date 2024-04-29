using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quiz_Builder_Persistence;
using QuizBuilder.Core.Application;

namespace QuizBuilder.Domain.Test
{
    public class TestSetup : IAsyncLifetime
    {
        public IMapper Mapper { get; private set; }
        public DataContext DataContext { get; private set; }

        public async Task InitializeAsync()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(MappingProfiles.All);
            });

            Mapper = configuration.CreateMapper();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            DataContext = new DataContext(options);

            await DataContext.Database.EnsureCreatedAsync();
            Seed();
        }

        private void Seed()
        {
            var users = new List<AppUser>()
            {
                new() {Email="user1@example.com",UserName = "user1", DisplayName = "user1"},
                new() {Email="user2@example.com",UserName = "user2", DisplayName = "User2"},
                new() {Email="user3@example.com",UserName = "user3", DisplayName = "User3"}
            };

            DataContext.Users.AddRange(users);
            DataContext.SaveChanges();
        }

        public Task DisposeAsync()
        {
            if (DataContext!= null)
            {
                DataContext.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}

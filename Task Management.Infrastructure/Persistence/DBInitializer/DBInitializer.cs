using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task_Management.Domain.Entities;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Persistence.DBInitializer
{
    public static class DBInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            AppDbContext context = serviceProvider.GetService<AppDbContext>()
                ?? throw new ArgumentNullException(nameof(AppDbContext));

            try
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Count() > 0)
                    await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Database migration failed: " + ex.Message);
            }

            if (!context.Projects.Any())
            {
                context.Projects.Add(new Project()
                {
                    ProjectName = "Default Project",
                    Description = "Initial seeded project",
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
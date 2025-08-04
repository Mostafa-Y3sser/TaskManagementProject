using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Extensions;
using Task_Management.Infrastructure.Persistence.Repositories;

namespace Task_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepository<TaskItem>, TaskRepository>();
            builder.Services.AddScoped<IRepository<Board>, BoardRepository>();
            builder.Services.AddScoped<IRepository<Project>, ProjectRepository>();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Management.Application.Interfaces;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Persistence.Data;
using Task_Management.Infrastructure.Persistence.Repositories;
using Task_Management.Infrastructure.Services;

namespace Task_Management.Infrastructure.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IJwtTokenService, JwtTokenServices>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<TaskItem>, TaskRepository>();
            services.AddScoped<IRepository<Board>, BoardRepository>();
            services.AddScoped<IRepository<Project>, ProjectRepository>();
        }
    }
}
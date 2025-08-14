using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Application.Extensions;
using Task_Management.Infrastructure.Extensions;
using Task_Management.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task_Management.Middlewares;
using Task_Management.Application.Interfaces;
using Task_Management.Infrastructure.Services;

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
            builder.Services.AddApplicationServices();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepository<TaskItem>, TaskRepository>();
            builder.Services.AddScoped<IRepository<Board>, BoardRepository>();
            builder.Services.AddScoped<IRepository<Project>, ProjectRepository>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}

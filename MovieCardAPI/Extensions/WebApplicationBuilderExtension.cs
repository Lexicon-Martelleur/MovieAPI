using MovieCardAPI.Model.Service;
using MovieCardAPI.Model.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Utility;
using MovieCardAPI.Constants;
using System.Reflection.Metadata;
using MovieCardAPI.Infrastructure.Repositories;

namespace MovieCardAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddControllersExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(configure =>
        {
            configure.ReturnHttpNotAcceptable = true;
        }).AddApplicationPart(typeof(AssemblyReference).Assembly);
    }

    public static void AddDBServiceExtension(this WebApplicationBuilder builder)
    {

        var connectionString = builder.Configuration.GetConnectionString(
            "DefaultConnection"
        ) ?? throw new InvalidOperationException("Default connection string not found.");

        builder.Services.AddDbContext<MovieContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Connection String: {connectionString}");
        }
    }

    public static void AddCustomLoggingExtension(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/info.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Services.AddSerilog();
    }

    public static void AddGlobalExceptionHandlingExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
    }

    public static void AddApplicationServicesExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMovieService, MovieService>();

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();

        builder.Services.AddScoped<Lazy<IMovieRepository>>(provider => 
            new (() => provider.GetRequiredService<IMovieRepository>()));
        
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddTransient<IMapper, Mapper>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }

    public static void AddSwaggerServiceExtension(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void AddCORSPolicyExtension(this WebApplicationBuilder builder)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AppConfig.CorsPolicies.Dev, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy(AppConfig.CorsPolicies.Prod, builder =>
            {
                builder.WithOrigins("https://my-movie-card.org")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}

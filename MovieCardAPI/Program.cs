
using MovieCardAPI.Model.Service;
using MovieCardAPI.Model.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.Extensions;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI;

/**
 * TODO Configure WebApp and HTTP Pipeline
 * 1. Log sensitive data in development.
 * 2. Move Program Functions for building
 * and configuring application to extension methods.
 */
public class Program
{
    private static readonly (string Dev, string Prod) CorsPolicies = (
        Dev: "DevCorsPolicy",
        Prod: "ProdCorsPolicy"
    );

    public static async Task Main(string[] args)
    {
        var app = CreateWebApplication(args);
        TestDBConnection(app);
        await ConfigureWebApplicationPipeline(app);
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AddDBService(builder);
        AddCustomLogging(builder);
        AddGlobalErrorHandling(builder);
        AddAppServices(builder);
        builder.Services.AddControllers();
        AddSwaggerService(builder);
        AddCorsPolicy(builder);
        return builder.Build();
    }

    private static void AddDBService (WebApplicationBuilder builder) {

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

    private static void AddCustomLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("Logs/info.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Services.AddSerilog();
    }

    private static void AddGlobalErrorHandling(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                if (context.Exception is NotImplementedException)
                {
                    context.ProblemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                    context.ProblemDetails.Title = "Resource not implemented";
                    context.ProblemDetails.Detail = context.Exception.Message;
                }
                else if (context.Exception != null)
                {
                    context.ProblemDetails.Status = StatusCodes.Status500InternalServerError;
                    context.ProblemDetails.Title = "An unexpected error occurred";
                    context.ProblemDetails.Detail = "Please try again later.";
                }
            };
        });
    }

    private static void AddAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddTransient<IMapper, Mapper>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }

    private static void AddSwaggerService(WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void TestDBConnection(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MovieContext>();
        try
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.CloseConnection();
            Console.WriteLine("Successfully connected to the database.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to connect to the database: {e.Message}");
        }
    }

    private static void AddCorsPolicy(WebApplicationBuilder builder)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Dev, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            options.AddPolicy(CorsPolicies.Prod, builder =>
            {
                builder.WithOrigins("https://my-movie-card.org")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private async static Task ConfigureWebApplicationPipeline(WebApplication app)
    {
        UseCorsPolicy(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.UseDataSeedAsync();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void UseCorsPolicy(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseCors(CorsPolicies.Dev);
        }
        else
        {
            app.UseCors(CorsPolicies.Prod);
        }
    }
}


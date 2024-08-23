
using MovieCardAPI.Model.Service;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.DB;
using Microsoft.EntityFrameworkCore;

namespace MovieCardAPI;

/**
 * TODO
 * 1. Set up global error management.
 * 2. Set up custom logging.
 * 3. Update model classes visibility level to internal if possible.
 * 4. Seed data only in development using
 *    async main, extension method; 
 *    Use University project as blueprint.
 * 5. Seed data with faker package Bogus.
 * 6. Log sensitive data in development.
 */
public class Program
{
    public static void Main(string[] args)
    {
        var app = CreateWebApplication(args);
        TestDBConnection(app);
        ConfigureWebApplicationPipeline(app);
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AddDBService(builder);

        AddAppServices(builder);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    private static void AddDBService (WebApplicationBuilder builder) {

        var connectionString = builder.Configuration.GetConnectionString(
            "DefaultConnection"
        ) ?? throw new InvalidOperationException("Connection string 'MovieDB1' not found.");

        builder.Services.AddDbContext<MovieContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Connection String: {connectionString}");
        }
    }

    private static void AddAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
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

    private static void ConfigureWebApplicationPipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

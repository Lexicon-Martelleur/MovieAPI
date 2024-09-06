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

    public static void AddApplicationDependenciesExtension(this WebApplicationBuilder builder)
    {
        AddApplicationUtilities(builder.Services);
        AddApplicationServices(builder.Services);
        AddApplicationRepositories(builder.Services);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }
    }

    private static void AddApplicationUtilities(IServiceCollection collection)
    {
        collection.AddScoped<IMapper, Mapper>();
    }

    private static void AddApplicationRepositories(IServiceCollection collection)
    {
        collection.AddScoped<IUnitOfWork, UnitOfWork>();
        collection.AddScoped<IActorRepository, ActorRepository>();
        collection.AddScoped<IContactInformationRepository, ContactInformationRepository>();
        collection.AddScoped<IDirectorRepository, DirectorRepository>();
        collection.AddScoped<IGenreRepository, GenreRepository>();
        collection.AddScoped<IMovieRepository, MovieRepository>();
        collection.AddScoped<IMovieGenreRepository, MovieGenreRepository>();
        collection.AddScoped<IMovieRoleRepository, MovieRoleRepository>();

        collection.AddScoped<Lazy<IActorRepository>>(provider => new(
            () => provider.GetRequiredService<IActorRepository>()));
        
        collection.AddScoped<Lazy<IContactInformationRepository>>(provider => new(
            () => provider.GetRequiredService<IContactInformationRepository>()));
        
        collection.AddScoped<Lazy<IDirectorRepository>>(provider => new(
            () => provider.GetRequiredService<IDirectorRepository>()));
        
        collection.AddScoped<Lazy<IGenreRepository>>(provider => new(
            () => provider.GetRequiredService<IGenreRepository>()));
        
        collection.AddScoped<Lazy<IMovieRepository>>(provider => new(
            () => provider.GetRequiredService<IMovieRepository>()));
        
        collection.AddScoped<Lazy<IMovieGenreRepository>>(provider => new(
            () => provider.GetRequiredService<IMovieGenreRepository>()));
        
        collection.AddScoped<Lazy<IMovieRoleRepository>>(provider => new(
            () => provider.GetRequiredService<IMovieRoleRepository>()));
    }

    private static void AddApplicationServices(IServiceCollection collection)
    {
        collection.AddScoped<IServiceManager, ServiceManager>();
        collection.AddScoped<IMovieService, MovieService>();

        collection.AddScoped<Lazy<IMovieService>>(provider => new(
            () => provider.GetRequiredService<IMovieService>()));
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

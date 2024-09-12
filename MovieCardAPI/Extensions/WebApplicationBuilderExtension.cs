using MovieCardAPI.Model.Service;
using MovieCardAPI.Model.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Utility;
using MovieCardAPI.Constants;
using System.Reflection.Metadata;
using MovieCardAPI.Infrastructure.Repositories;
using MovieCardAPI.Presentation.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        collection.AddScoped<ICustomMapper, CustomMapper>();
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
        collection.AddAutoMapper(typeof(AutoMapperProfile));
        collection.AddWithLazy<IServiceManager, ServiceManager>();
        collection.AddWithLazy<IMovieService, MovieService>();
        collection.AddWithLazy<IActorService, ActorService>();
        collection.AddWithLazy<IDirectorService, DirectorService>();
        collection.AddWithLazy<IGenreService, GenreService>();
        collection.AddWithLazy<IAuthenticationService, AuthenticationService>();

        //collection.AddScoped<IMovieService, MovieService>();
        //collection.AddScoped<IActorService, ActorService>();
        //collection.AddScoped<IDirectorService, DirectorService>();
        //collection.AddScoped<IGenreService, GenreService>();
        //collection.AddScoped<IAuthenticationService, AuthenticationService>();

        //collection.AddScoped<Lazy<IMovieService>>(provider => new(
        //    () => provider.GetRequiredService<IMovieService>()));

        //collection.AddScoped<Lazy<IActorService>>(provider => new(
        //    () => provider.GetRequiredService<IActorService>()));

        //collection.AddScoped<Lazy<IDirectorService>>(provider => new(
        //    () => provider.GetRequiredService<IDirectorService>()));

        //collection.AddScoped<Lazy<IGenreService>>(provider => new(
        //    () => provider.GetRequiredService<IGenreService>()));

        //collection.AddScoped<Lazy<IAuthenticationService>>(provider => new(
        //    () => provider.GetRequiredService<IAuthenticationService>()));
    }

    // TODO Use this for services and repositories.
    internal static void AddWithLazy<IServiceType, ServiceType>(
        this IServiceCollection collection,
        string scope = "scope")
        where ServiceType : class, IServiceType
        where IServiceType : class
    {
        switch (scope)
        {
            case "singleton":
                collection.AddSingleton<IServiceType, ServiceType>();
                collection.AddSingleton<Lazy<IServiceType>>(provider => new(
                    () => provider.GetRequiredService<IServiceType>()));
                break;
            case "transient":
                collection.AddTransient<IServiceType, ServiceType>();
                collection.AddTransient<Lazy<IServiceType>>(provider => new(
                    () => provider.GetRequiredService<IServiceType>()));
                break;
            default:
                collection.AddScoped<IServiceType, ServiceType>();
                collection.AddScoped<Lazy<IServiceType>>(provider => new(
                    () => provider.GetRequiredService<IServiceType>()));
                break;
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
                    .AllowAnyHeader()
                    .WithExposedHeaders(CustomHeader.Pagination); ;
            });

            options.AddPolicy(AppConfig.CorsPolicies.Prod, builder =>
            {
                builder.WithOrigins("https://my-movie-card.org")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(CustomHeader.Pagination); ;
            });
        });
    }

    public static void AddIdentityCoreExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddDataProtection();

        builder.Services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 8;

        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<MovieContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddAuthenticationExtension(this WebApplicationBuilder builder)
    {

        var secretkey = AppConfig.GetSecretKey(builder.Configuration);
        ArgumentNullException.ThrowIfNull(secretkey, nameof(secretkey));

        var jwtConfiguration = new JWTConfiguration();
        builder.Configuration.Bind(JWTConfiguration.Section, jwtConfiguration);

        builder.Services.AddSingleton<IJWTConfiguration>(jwtConfiguration);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
            };

        });
    }

    public static void AddAuthorizationExtension(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin")
                      .RequireClaim(ClaimTypes.NameIdentifier)
                      .RequireClaim(ClaimTypes.Role));

            options.AddPolicy("UserPolicy", policy =>
                policy.RequireRole("User"));
        });
    }
}

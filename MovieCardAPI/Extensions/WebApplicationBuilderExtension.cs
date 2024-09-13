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
        collection.AddWithLazy<IActorRepository, ActorRepository>();
        collection.AddWithLazy<IContactInformationRepository, ContactInformationRepository>();
        collection.AddWithLazy<IDirectorRepository, DirectorRepository>();
        collection.AddWithLazy<IGenreRepository, GenreRepository>();
        collection.AddWithLazy<IMovieRepository, MovieRepository>();
        collection.AddWithLazy<IMovieGenreRepository, MovieGenreRepository>();
        collection.AddWithLazy<IMovieRoleRepository, MovieRoleRepository>();
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
    }

    private static void AddWithLazy<IServiceType, ServiceType>(
        this IServiceCollection collection,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where ServiceType : class, IServiceType
        where IServiceType : class
    {
        collection.Add(new ServiceDescriptor(
            typeof(IServiceType),
            typeof(ServiceType),
            lifetime));

        collection.Add(new ServiceDescriptor(
            typeof(Lazy<IServiceType>),
            provider => new Lazy<IServiceType>(
                () => provider.GetRequiredService<IServiceType>()),
            lifetime));
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
                    ClockSkew = TimeSpan.Zero,
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

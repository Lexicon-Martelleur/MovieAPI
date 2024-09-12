using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Infrastructure.Seeds;
using MovieCardAPI.Constants;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MovieCardAPI.Model.Exeptions;
using Microsoft.AspNetCore.Identity;
using MovieCardAPI.Entities;

namespace MovieCardAPI.Extensions;

public static class WebApplicationExtension
{
    public static void UseConfigValidationExtension(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        AppConfig.Validate(configuration);
    }

    public static async Task UseDataSeedAsyncExtension(this IApplicationBuilder applicationBuilder)
    {   
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var context = serviceProvider.GetRequiredService<MovieContext>();
        await context.Database.MigrateAsync();
        await SeedMovieDB.RunAsync(context, configuration, userManager, roleManager);
    }

    public static void UseCORSPolicyExtension(this WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseCors(AppConfig.CorsPolicies.Dev);
        }
        else
        {
            application.UseCors(AppConfig.CorsPolicies.Prod);
        }
    }

    public static void TestDBConnectionExtension(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
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

    public static void UseGlobalExceptionHandlerExtension(this WebApplication application)
    {
        application.UseExceptionHandler(apperror =>
        {
            apperror.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature == null) { return; }
                
                var problemDetailsFactory = application.Services.GetRequiredService<ProblemDetailsFactory>();

                await HandleGlobalErrors(
                    context,
                    contextFeature.Error,
                    problemDetailsFactory);
            });
        });
    }

    private static async Task HandleGlobalErrors(
        HttpContext context,
        Exception error,
        ProblemDetailsFactory problemDetailsFactory)
    {
        ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
            context,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Internal Server Error",
            detail: error.Message);

        switch (error)
        {
            case NotFoundException exception:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: StatusCodes.Status404NotFound,
                    title: exception.Title,
                    detail: exception.Message);
                break;
            case DomainUnableOperationException exception:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: exception.Title,
                    detail: exception.Message);
                break;
            case NotImplementedException exception:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: StatusCodes.Status503ServiceUnavailable,
                    title: "Resource not implemented",
                    detail: exception.Message);
                break;
            default: break;
        }
        context.Response.StatusCode = problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}

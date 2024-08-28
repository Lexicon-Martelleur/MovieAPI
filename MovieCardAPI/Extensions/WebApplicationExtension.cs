using Microsoft.EntityFrameworkCore;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.DB.Seeds;
using MovieCardAPI.Constants;

namespace MovieCardAPI.Extensions;

public static class WebApplicationExtension
{
    public static async Task UseDataSeedAsyncExtension(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<MovieContext>();
        await context.Database.MigrateAsync();
        await SeedMovieDB.RunAsync(context);
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
}

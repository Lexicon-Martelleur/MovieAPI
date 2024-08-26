using Microsoft.EntityFrameworkCore;
using MovieCardAPI.DB.Contexts;
using MovieCardAPI.DB.Seeds;

namespace MovieCardAPI.Extensions;

public static class WebApplicationExtension
{
    public static async Task UseDataSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        Console.WriteLine("Seeding data");
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<MovieContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        await SeedMovieDB.RunAsync(context);
    }
}

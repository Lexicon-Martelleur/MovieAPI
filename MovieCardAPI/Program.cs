using MovieCardAPI.Extensions;

namespace MovieCardAPI;

/**
 * TODO Refine/Check how sensitive data is logged in Dev vs Prod.
 */
public class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateWebApplication(args);
        app.TestDBConnectionExtension();
        await ConfigureWebApplicationPipeline(app);
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddControllersExtension();
        builder.AddDBServiceExtension();
        builder.AddCustomLoggingExtension();
        builder.AddGlobalExceptionHandlingExtension();
        builder.AddApplicationDependenciesExtension();
        builder.AddSwaggerServiceExtension();
        builder.AddCORSPolicyExtension();
        builder.AddIdentityCoreExtension();
        builder.AddAuthenticationExtension();
        return builder.Build();
    }

    private async static Task ConfigureWebApplicationPipeline(WebApplication app)
    {
        app.UseConfigValidationExtension();
        app.UseCORSPolicyExtension();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.UseDataSeedAsyncExtension();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseGlobalExceptionHandlerExtension();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static string AddDebug()
    {
        Console.WriteLine("clean...");
        Console.WriteLine("update");
        return "Adding Debug";
    }
}

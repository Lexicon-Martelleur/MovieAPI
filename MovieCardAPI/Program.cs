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
        builder.AddGlobalErrorHandlingExtension();
        builder.AddApplicationServicesExtension();
        builder.AddSwaggerServiceExtension();
        builder.AddCORSPolicyExtension();
        return builder.Build();
    }

    private async static Task ConfigureWebApplicationPipeline(WebApplication app)
    {
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
            app.UseExceptionHandler();
        }
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

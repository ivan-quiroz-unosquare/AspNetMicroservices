using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");

            // Add services to the container.
            builder.Services.AddOcelot()
                .AddCacheManager(settings =>
                {
                    settings.WithDictionaryHandle();
                });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}

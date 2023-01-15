using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Quotes.API.Enrichers;

Log.Logger = new LoggerConfiguration()
    .Enrich.With(new ThreadIdEnricher())
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .Enrich.With(new ThreadIdEnricher())
        .Enrich.WithProperty("ApplicationName", "Quotes.API")
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();


    app.Run();
}
catch (Exception ex)
{
     Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
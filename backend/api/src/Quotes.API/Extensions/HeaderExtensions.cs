namespace Quotes.API.Extensions;

public static class HeaderExtensions
{
    public static IServiceCollection AddAcceptHeader(this IServiceCollection services)
    {
        services.AddTransient<HeaderMiddleware>();
        return services;
    }
    public static IServiceCollection AddMediaTypeHeader(this IServiceCollection services)
    {
        services.AddTransient<MediaTypeMiddleware>();
        return services;
    }
    public static IApplicationBuilder UseAcceptHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<HeaderMiddleware>();
        return app;
    }
    public static IApplicationBuilder UseMediaTypeHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<MediaTypeMiddleware>();
        return app;
    }
}

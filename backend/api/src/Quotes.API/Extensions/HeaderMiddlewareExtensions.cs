namespace Quotes.API.Extensions;

public static class HeaderMiddlewareExtensions
{
    public static IServiceCollection AddAcceptHeader(this IServiceCollection services)
    {
        services.AddTransient<AcceptHeaderMiddleware>();
        return services;
    }
    public static IServiceCollection AddMediaTypeHeader(this IServiceCollection services)
    {
        services.AddTransient<MediaTypeHeaderMiddleware>();
        return services;
    }
    public static IApplicationBuilder UseAcceptHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<AcceptHeaderMiddleware>();
        return app;
    }
    public static IApplicationBuilder UseMediaTypeHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<MediaTypeHeaderMiddleware>();
        return app;
    }
}

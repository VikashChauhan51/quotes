using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Quotes.API.Authorization;
using Quotes.API.Configurations;
using Quotes.API.Constants;
using Quotes.API.DbContexts;
using Quotes.API.Helpers;
using Quotes.API.Policies;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Quotes.API.Extensions;

internal static class HostingExtensions
{

    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddControllers(configure =>
        {
            configure.ReturnHttpNotAcceptable = true;
            configure.OutputFormatters.RemoveType<StringOutputFormatter>();
            configure.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
                    StatusCodes.Status406NotAcceptable));
            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
                    StatusCodes.Status415UnsupportedMediaType));
            configure.Filters.Add(
                 new ProducesResponseTypeAttribute(
           StatusCodes.Status400BadRequest));
            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
           StatusCodes.Status401Unauthorized));
            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
           StatusCodes.Status403Forbidden));
            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
                    StatusCodes.Status429TooManyRequests));
            configure.Filters.Add(
                new ProducesResponseTypeAttribute(
                    StatusCodes.Status500InternalServerError));
            configure.Filters.Add(new ProducesDefaultResponseTypeAttribute());
            configure.Filters.Add(
              new ConsumesAttribute(HeaderKeys.Json));
            configure.Filters.Add(
             new ProducesAttribute(HeaderKeys.Json, HeaderKeys.HalJson)); 

        }).ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
                new BadRequestObjectResult(context.ModelState)
                {
                    ContentTypes =
                    {
                        HeaderKeys.Json
                    }
                };
        }).AddNewtonsoftJson(setupAction =>
        {
            setupAction.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            setupAction.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

        builder.Services.Configure<MvcOptions>(config =>
        {
            config.ReturnHttpNotAcceptable = true;
            var newtonsoftJsonOutputFormatter = config.OutputFormatters
                  .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

            if (newtonsoftJsonOutputFormatter != null)
            {
                newtonsoftJsonOutputFormatter.SupportedMediaTypes
                    .Add(HeaderKeys.HalJson);
            }

            var jsonOutputFormatter = config.OutputFormatters
                  .OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();

            if (jsonOutputFormatter != null)
            {
                if (jsonOutputFormatter.SupportedMediaTypes.Contains(HeaderKeys.TextJson))
                {
                    jsonOutputFormatter.SupportedMediaTypes.Remove(HeaderKeys.TextJson);
                }
            }

        });

        builder.Services.AddApiVersioning(setupAction =>
        {
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(1, 0);
            setupAction.ReportApiVersions = true;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        builder.Services.AddAcceptHeader();
        builder.Services.AddMediaTypeHeader();

        builder.Services.AddDbContext<QuotesContext>(options =>
        {
            var sqlConfig = new SqlConfig();
            builder.Configuration.GetSection(ConfigSessions.SqlServerConfig).Bind(sqlConfig);
            sqlConfig.Credentials = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.DbCredentialsKey)!;
            var connectionString = ConnectionStringBuilder.BuildConnectionString(sqlConfig!);
            options.UseSqlServer(connectionString, setupAction => 
            {
                setupAction.EnableRetryOnFailure(3);
            });
        });

        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IAuthorizationHandler, MustOwnQuoteHandler>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IValidator<QuoteModel>, QuoteValidator>();
        builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
        builder.Services.AddOptions();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var authConfig = new AuthenticationConfig();
        builder.Configuration.GetSection(ConfigSessions.AuthenticationConfig).Bind(authConfig);
        authConfig.ClientId = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.AuthenticationClientIdKey)!;
        authConfig.ClientSecret = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.AuthenticationClientSecretKey)!;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

              .AddOAuth2Introspection(options =>
              {
                 

                  options.Authority = authConfig.Authority;
                  options.ClientId = authConfig.ClientId;
                  options.ClientSecret = authConfig.ClientSecret;
                  options.NameClaimType = authConfig.NameClaimType;
                  options.RoleClaimType = authConfig.RoleClaimType;
              });


        builder.Services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.AddPolicy(
                "UserCanAddQuote", AuthorizationPolicies.CanAddQuote());
            authorizationOptions.AddPolicy(
                "ClientApplicationCanWrite", policyBuilder =>
                {
                    policyBuilder.RequireClaim("scope", "quoteapi.write");
                });
            authorizationOptions.AddPolicy(
                "MustOwnQuote", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddRequirements(new MustOwnQuoteRequirement());

                });
        });

        return builder.Build();
    }
    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(
                    Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(
                    Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();

            });
        }
        app.ConfigureExceptionHandler(Log.Logger);
        app.UseMediaTypeHeader();
        app.UseAcceptHeader();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}

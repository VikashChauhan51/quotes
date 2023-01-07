﻿using Dapr.Client;
using FluentValidation;
using Google.Api;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Quotes.API.Authorization;
using Quotes.API.Configurations;
using Quotes.API.Constants;
using Quotes.API.DbContexts;
using Quotes.API.HealthCheck;
using Quotes.API.Helpers;
using Quotes.API.Policies;
using Quotes.API.Services;
using Serilog;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Reflection;
using System.Threading.RateLimiting;

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

        builder.Services.AddHealthChecks()
       .AddCheck<ApiHealthCheck>("ApiHealthCheck");

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
            setupAction.SerializerSettings.Formatting = Formatting.Indented;

            setupAction.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
            setupAction.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            setupAction.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            setupAction.SerializerSettings.Converters.Add(new StringEnumConverter());
        }).AddDapr();

        builder.Services.Configure<DaprConfig>(options =>
        {
            var daprConfig = new DaprConfig();
            builder.Configuration.GetSection(ConfigSessions.DaprConfig).Bind(daprConfig);
            options.SecretstoreName = daprConfig.SecretstoreName;
            options.StatestoreName = daprConfig.StatestoreName;
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
        builder.Services.AddRateLimiter(options =>
        {
            var rateLimitConfig = new TokenBucketRateLimiterConfig();
            builder.Configuration.GetSection(ConfigSessions.TokenBucketRateLimiterConfig).Bind(rateLimitConfig);
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = (context, cancellationToken) =>
            {

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return new ValueTask();
            };

            options.AddPolicy(ApiConstants.UserBasedRateLimitingPolicy, new UserBasedRateLimitingPolicy(rateLimitConfig));

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var concurrencyRateLimitConfig = new ConcurrencyLimiterConfig();
                builder.Configuration.GetSection(ConfigSessions.ConcurrencyRateLimiterConfig).Bind(concurrencyRateLimitConfig);
                var host = context.Request.Headers.Host.ToString();
                return RateLimitPartition.GetConcurrencyLimiter(host, _ => new ConcurrencyLimiterOptions
                {
                    PermitLimit = concurrencyRateLimitConfig.PermitLimit,
                    QueueProcessingOrder = (QueueProcessingOrder)concurrencyRateLimitConfig.QueueProcessingOrder,
                    QueueLimit = concurrencyRateLimitConfig.QueueLimit
                });
            });

        });
        var daprClient = new DaprClientBuilder().Build();
        var daprConfig = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<DaprConfig>>();
        builder.Services.AddDbContext<QuotesContext>(options =>
        {           
            var sqlConfig = new SqlConfig();
            builder.Configuration.GetSection(ConfigSessions.SqlServerConfig).Bind(sqlConfig);
            var keyName = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.DbCredentialsKey)!;
            var secretKeys = daprClient.GetSecretAsync(daprConfig.Value.SecretstoreName, keyName).Result;
            sqlConfig.Credentials = secretKeys[keyName]; 
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
        builder.Services.AddSingleton<ISecretStore, SecretStore>();
        builder.Services.AddSingleton<IContentStore<string>, ContentStore<string>>();


        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        var authConfig = new AuthenticationConfig();
        builder.Configuration.GetSection(ConfigSessions.AuthenticationConfig).Bind(authConfig);
        var clientIdKey = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.AuthenticationClientIdKey)!;
        var clientSecretKey = (string)builder.Configuration.GetValue(typeof(string), SecretKeys.AuthenticationClientSecretKey)!;
        var secretKeys = daprClient.GetSecretAsync(daprConfig.Value.SecretstoreName, (string)builder.Configuration.GetValue(typeof(string), SecretKeys.AuthenticationKey)!).Result;
        authConfig.ClientId = secretKeys[clientIdKey];
        authConfig.ClientSecret = secretKeys[clientSecretKey];

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
               ApiConstants.CanAddQuoteAuthorizationPolicy, AuthorizationPolicies.CanAddQuote());
            authorizationOptions.AddPolicy(
                ApiConstants.ClientCanWriteAuthorizationPolicy, policyBuilder =>
                {
                    var scops = builder.Configuration.GetSection(ConfigSessions.ClientApplicationCanWriteScopes)
                                .GetChildren()?.Select(x => x.Value)?.ToArray();
                    policyBuilder.RequireClaim(JwtClaimTypes.Scope, scops!);
                });
            authorizationOptions.AddPolicy(
                ApiConstants.MustOwnQuoteAuthorizationPolicy, policyBuilder =>
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
        app.UseRateLimiter();
        app.MapHealthChecks("/health/startup").AllowAnonymous();
        app.MapHealthChecks("/healthz").AllowAnonymous();
        app.MapHealthChecks("/ready").AllowAnonymous();
        app.MapControllers().RequireRateLimiting(ApiConstants.UserBasedRateLimitingPolicy);

        return app;
    }
}

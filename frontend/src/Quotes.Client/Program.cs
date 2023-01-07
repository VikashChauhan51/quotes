using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Quotes.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
var apiEndpoint = builder.Configuration.GetValue<string>("ApiEndpoint");
var identityEndpoint = builder.Configuration.GetValue<string>("IdentityEndpoint");
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("authPolicy", builder => 
    {
        builder.WithOrigins(new string[] { apiEndpoint!, identityEndpoint! })
        .AllowAnyMethod().AllowAnyHeader();
    });
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAccessTokenManagement();


builder.Services.AddHttpClient<IQuoteAuthenticationService, QuoteAuthenticationService>(client =>
{
    client.BaseAddress = new Uri(identityEndpoint!);
});
builder.Services.AddHttpClient<IRootService, RootService>(client =>
{
    client.BaseAddress = new Uri(apiEndpoint!);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.HalJson);
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient<ISearchService, SearchService>(client =>
{
    client.BaseAddress = new Uri(apiEndpoint!);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.HalJson);
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient<IQuoteService, QuoteService>(client =>
{
    client.BaseAddress = new Uri(apiEndpoint!);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.Json);
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.HalJson);
}).AddUserAccessTokenHandler();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = identityEndpoint!;
    options.ClientId = "quoteclient";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.RequireHttpsMetadata = true;
    options.Scope.Add("roles");
    options.Scope.Add("quoteapi.fullaccess");
    options.Scope.Add("quoteapi.read");
    options.Scope.Add("quoteapi.write");
    options.Scope.Add("offline_access");
    options.ClaimActions.MapJsonKey("role", "role");

    options.TokenValidationParameters = new()
    {
        NameClaimType = "given_name",
        RoleClaimType = "role",
    };
});

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("UserCanAddQuote",
        new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("AddQuote")
                .Build());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

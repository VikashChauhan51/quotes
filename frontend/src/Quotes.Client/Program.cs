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

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAccessTokenManagement();

builder.Services.AddHttpClient<IQuoteAuthenticationService, QuoteAuthenticationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});
builder.Services.AddHttpClient<IRootService, RootService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7149/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.HalJson);
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient<ISearchService, SearchService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7149/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, HeaderKeys.HalJson);
}).AddUserAccessTokenHandler();

builder.Services.AddHttpClient<IQuoteService, QuoteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7149/");
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
    options.Authority = "https://localhost:5001/";
    options.ClientId = "quoteclient";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

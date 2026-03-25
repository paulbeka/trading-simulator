using Database.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TradingApi.Hubs;
using TradingApi.Kafka.Config;
using TradingApi.Kafka.Consumer;
using TradingApi.Kafka.Consumers;
using TradingApi.Options;
using TradingApi.Services;
using TradingApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Controllers + OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Config
builder.Services.Configure<JwtConfig>(
    builder.Configuration.GetSection(JwtConfig.SectionName));

builder.Services.Configure<PnlKafkaSettings>(
    builder.Configuration.GetSection("PnlKafka"));

builder.Services.Configure<MarketKafkaSettings>(
    builder.Configuration.GetSection("MarketKafka"));

builder.Services.Configure<PolygonConfig>(options =>
{
    options.ApiKey = builder.Configuration["POLYGON_API"]
        ?? throw new Exception("POLYGON_API not set");
});

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DbConnection")
    ?? throw new InvalidOperationException("Connection string 'DbConnection' is missing.");

builder.Services.AddDbContext<TradingDbContext>(options =>
    options.UseNpgsql(connectionString));

// Other services
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IPolygonService, PolygonService>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<PnlConsumer>();
builder.Services.AddHostedService<MarketDataConsumer>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IMarketDataService, MarketDataService>();

// JWT config
var jwtConfig = builder.Configuration
    .GetSection(JwtConfig.SectionName)
    .Get<JwtConfig>()
    ?? throw new InvalidOperationException("JwtConfig section is missing.");

if (string.IsNullOrWhiteSpace(jwtConfig.Key))
    throw new InvalidOperationException("JWT key is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig.Key)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/pnlHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PnlHub>("/pnlHub");

app.Run();
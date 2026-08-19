using FluentValidation;
using FluentValidation.AspNetCore;
using FreelanceAPI.Data;
using FreelanceAPI.Filters;
using FreelanceAPI.Helpers;
using FreelanceAPI.Interfaces;
using FreelanceAPI.Models;
using FreelanceAPI.OpenApi.Transformers;
using FreelanceAPI.Repositories;
using FreelanceAPI.Repositories.Interfaces;
using FreelanceAPI.Requests;
using FreelanceAPI.Services.implementation;
using FreelanceAPI.Services.Interface;
using FreelanceMarketplace.API.Data;
using FreelanceMarketplace.API.Enums;
using Google.GenAI;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Scalar.AspNetCore;
using System.IO.Compression;
using System.Runtime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IServiceRepository, ServiceRepo>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
    };
});


//builder.Services.AddRateLimiter(Options =>
//{
//    Options.AddFixedWindowLimiter("default", Options =>
//    {
//        Options.PermitLimit = 100;
//        Options.Window = TimeSpan.FromMinutes(1);
//        Options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        Options.QueueLimit = 10;
//    }); 

//});
//builder.Services.AddRateLimiter(Options =>
//{
//    Options.AddSlidingWindowLimiter("default", Options =>
//    {
//        Options.PermitLimit = 100;
//        Options.Window = TimeSpan.FromMinutes(1);
//        Options.SegmentsPerWindow = 10;
//        Options.AutoReplenishment = true;
//        Options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        Options.QueueLimit = 10;
//    });
//});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: "global",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 10,
                AutoReplenishment = true,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }));
});
builder.Services.AddResponseCompression(Options =>
{
    Options.EnableForHttps = true;
    Options.Providers.Add<GzipCompressionProvider>();
    Options.ExcludedMimeTypes = new[] {
       "application/json",
       "text/plain",
       "text/html",
         "application/xml",
    };
});
builder.Services.Configure<GzipCompressionProviderOptions>(Options =>
{
    Options.Level = CompressionLevel.Fastest;
});

//l1
builder.Services.AddMemoryCache(Options =>
{
    Options.SizeLimit = 100;
});
//l2
builder.Services.AddStackExchangeRedisCache(Options =>
{
    Options.Configuration = builder.Configuration.GetConnectionString("Redis");
    Options.InstanceName = "FreelanceAPI_";
});
//l3
builder.Services.AddDistributedSqlServerCache(Options =>
{
    Options.ConnectionString = builder.Configuration.GetConnectionString("SqlCache");
    Options.SchemaName= "dbo";
    Options.TableName = "CacheEntries";
});
builder.Services.AddHybridCache(Options =>
{
    Options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(30),//l1,l2
        LocalCacheExpiration = TimeSpan.FromSeconds(5)
    };
});
// Identity Configuration
builder.Services.AddIdentity<User, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Controllers & JSON Serializer
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CalculateActionTimeFilter>();
    //options.Filters.Add<GlobalExceptionFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Authentication & JWT
builder.Services.Configure<JwtSettings>(    builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["issuer"],
        ValidAudience = jwtSettings["audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!))
    };
});


builder.Services.AddAuthorization();

// FluentValidation (تصحيح السطر المسبب للمشكلة)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddOpenApi();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<VersionInfoTransformer>();

    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.Configure<GroqOptions>(
    builder.Configuration.GetSection(GroqOptions.SectionName));

// Typed HttpClient for Groq, wired via HttpClientFactory
builder.Services.AddHttpClient<IGroqChatService, GroqChatService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<GroqOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.ApiKey);
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedRolesAsync(services);
}

// Development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Title = "Freelance Marketplace API";
    });
}

// Error Handling
app.UseExceptionHandler();
app.UseStatusCodePages();

// Security
app.UseHttpsRedirection();

// Performance
app.UseResponseCompression();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Rate Limiting
app.UseRateLimiter();

// Endpoints
app.MapControllers();
app.MapControllers();

app.Run();

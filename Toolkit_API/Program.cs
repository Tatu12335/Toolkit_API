using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using System.Linq.Expressions;
using System.Text;
using System.Threading.RateLimiting;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Infrastructure.Repositories;
using Toolkit_API.Infrastructure.Security;
using Toolkit_API.Infrastructure.Security.Jwt;
using Toolkit_API.Middleware;
using Toolkit_API.Infrastructure.Services;
using Toolkit_API.Infrastructure;


// Time spent on the project : 11hrs
var builder = WebApplication.CreateBuilder(args);
var connetionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
?? throw new InvalidOperationException("'DB_CONNECTION' not found");

// Add services to the container.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "Fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromSeconds(10),
                PermitLimit = 5,
            });
    });
    options.RejectionStatusCode = 429; // Too Many Requests
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    { 
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")))
        };
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(b =>
{
    b.AddConsole();
    b.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddTransient<IUserRepo, SqlUserRepo>(sp =>
    new SqlUserRepo(sp.GetRequiredService<IPasswordHasher>(), connetionString)
);
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<Login>();
builder.Services.AddTransient<CreateUser>();
builder.Services.AddTransient<FileHasher>();
builder.Services.AddTransient<IFileScanRepo,FileScanRepo>(sp =>
    new FileScanRepo(sp.GetRequiredService<FileHasher>(),connetionString)
);
builder.Services.AddTransient<FileScanOps>(sp =>
    new FileScanOps(sp.GetRequiredService<IFileScanRepo>(),sp.GetRequiredService<ICallExternalAPI>(), sp.GetRequiredService<HandleResult>())
);
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");

builder.Services.AddTransient<IGenerateToken, TokenGenerator>(sp =>

    new TokenGenerator(jwtKey)
);
builder.Services.AddHttpClient<ICallExternalAPI, ExternalCalls>();
builder.Services.AddTransient<HandleResult>();

var app = builder.Build();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();


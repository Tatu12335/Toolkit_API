using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Application.Settings;
using Toolkit_API.Infrastructure.Repositories;
using Toolkit_API.Infrastructure.Security;
using Toolkit_API.Infrastructure.Security.Jwt;
using Toolkit_API.Middleware;


// Time spent on the project : 3hrs
var builder = WebApplication.CreateBuilder(args);
var connetionString = builder.Configuration["DB_CONNECTION"]
?? throw new InvalidOperationException("'DB_CONNECTION' not found");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();



builder.Services.AddTransient<IUserRepo, SqlUserRepo>(sp =>
    new SqlUserRepo(sp.GetRequiredService<IPasswordHasher>(),connetionString)
);
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();


builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();

var jwtKey = builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddTransient<IGenerateToken, TokenGenerator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

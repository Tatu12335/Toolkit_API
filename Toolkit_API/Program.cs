using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Infrastructure.Repositories;
using Toolkit_API.Infrastructure.Security;
using Toolkit_API.Infrastructure.Security.Jwt;
using Toolkit_API.Middleware;


// Time spent on the project : 6hrs
var builder = WebApplication.CreateBuilder(args);
var connetionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
?? throw new InvalidOperationException("'DB_CONNECTION' not found");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging( b =>
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
    new SqlUserRepo(sp.GetRequiredService<IPasswordHasher>(),connetionString)
);
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<Login>();
builder.Services.AddTransient<CreateUser>();


/*builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables();*/

var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");

builder.Services.AddTransient<IGenerateToken, TokenGenerator>(sp =>

    new TokenGenerator(jwtKey)
);
var app = builder.Build();

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


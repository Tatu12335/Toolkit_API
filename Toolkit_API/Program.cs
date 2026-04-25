using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;
using System.Threading.RateLimiting;
using Toolkit_API.Application.Analysis;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.Application.Application_Services.Admin;
using Toolkit_API.Application.Application_Services.EmailServices;
using Toolkit_API.Application.Application_Services.FileOperations;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Policies;
using Toolkit_API.Infrastructure.Repositories;
using Toolkit_API.Infrastructure.Security;
using Toolkit_API.Infrastructure.Security.Jwt;
using Toolkit_API.Infrastructure.Services;
using Toolkit_API.Middleware;

// Time spent on the project : 37hrs 0min
var builder = WebApplication.CreateBuilder(args);
var connetionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
?? throw new InvalidOperationException("'DB_CONNECTION' not found");
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");

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
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<Login>();
builder.Services.AddTransient<CreateUser>();
builder.Services.AddTransient<FileHasher>();
builder.Services.AddHttpClient<ICallExternalAPI, ExternalCalls>();
builder.Services.AddTransient<HandleResult>();
builder.Services.AddTransient<IFileAnalysis, FileAnalysis>();
builder.Services.AddTransient<ExtractedStrings>();
builder.Services.AddTransient<IEmailServices, EmailServices>();
builder.Services.AddTransient<ZipPolicies>();
builder.Services.AddTransient<IZipHandler, HandleZip>();
builder.Services.AddTransient<HandleFolder>();
builder.Services.AddTransient<HandleZip>();
builder.Services.AddTransient<HandleResult>();
builder.Services.AddTransient<StaticFileAnalysis>();
builder.Services.AddTransient<FileAnalysisResult>();

builder.Services.AddTransient<ScoringAlg>(
    options => new ScoringAlg(options.GetRequiredService<IFileAnalysis>(),
    options.GetRequiredService<HandleResult>(),
    0.0,
    options.GetRequiredService<ExtractedStrings>()

));

builder.Services.AddTransient<IUserRepo, SqlUserRepo>(options =>
    new SqlUserRepo(options.GetRequiredService<IPasswordHasher>(), connetionString)
);

builder.Services.AddTransient<IAdminRepo, AdminRepository>(options =>
    new AdminRepository(connetionString)
);

builder.Services.AddTransient<HandleZIP>(options =>
    new HandleZIP(
    options.GetRequiredService<HandleZip>(),
    options.GetRequiredService<ZipPolicies>())
);

builder.Services.AddTransient<IGenerateToken, TokenGenerator>(options =>
    new TokenGenerator(jwtKey)
);

builder.Services.AddTransient<NewLetter>(options =>
    new NewLetter(options.GetRequiredService<IEmailServices>())
);

builder.Services.AddTransient<StaticFileAnalysis>(options =>
    new StaticFileAnalysis(options.GetRequiredService<IFileAnalysis>(),
        options.GetRequiredService<ScoringAlg>(),
        options.GetRequiredService<ExtractedStrings>()

    )
);

builder.Services.AddTransient<IFileScanRepo, FileScanRepo>(options =>
    new FileScanRepo(options.GetRequiredService<FileHasher>(),
    connetionString
    )
);

builder.Services.AddTransient<FileScanOps>(options =>
    new FileScanOps(options.GetRequiredService<IFileScanRepo>(),
    options.GetRequiredService<ICallExternalAPI>(),
    options.GetRequiredService<HandleResult>(),
    options.GetRequiredService<StaticFileAnalysis>(),
    options.GetRequiredService<FileHasher>(),
    options.GetRequiredService<HandleZIP>()
    )

);
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
/*builder.Services.AddTransient<IUserRepo, SqlUserRepo>(sp =>
    new SqlUserRepo(sp.GetRequiredService<IPasswordHasher>(), connetionString)
);
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<Login>();
builder.Services.AddTransient<CreateUser>();
builder.Services.AddTransient<FileHasher>();
builder.Services.AddTransient<IFileScanRepo, FileScanRepo>(sp =>
    new FileScanRepo(sp.GetRequiredService<FileHasher>(), connetionString)
);
builder.Services.AddTransient<IAdminRepo, AdminRepository>(sp =>
    new AdminRepository(connetionString)
);

builder.Services.AddTransient<AdminOperations>(sp =>
    new AdminOperations(sp.GetRequiredService<IAdminRepo>())
);

builder.Services.AddTransient<IZipHandler, Toolkit_API.Infrastructure.Services.HandleZip>();

builder.Services.AddTransient<ZipPolicies>();

builder.Services.AddTransient<Toolkit_API.Application.Application_Services.FileOperations.HandleZip>(sp =>
new Toolkit_API.Application.Application_Services.FileOperations.HandleZip(sp.GetRequiredService<IZipHandler>(), sp.GetRequiredService<ZipPolicies>()));

builder.Services.AddTransient<HandleFolder>();

builder.Services.AddTransient<FileScanOps>(sp => 
    sp.GetRequiredService<FileScanOps>()
);
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");

builder.Services.AddTransient<IGenerateToken, TokenGenerator>(sp =>

    new TokenGenerator(jwtKey)
);
builder.Services.AddHttpClient<ICallExternalAPI, ExternalCalls>();
builder.Services.AddTransient<HandleResult>();

builder.Services.AddTransient<IFileAnalysis, FileAnalysis>();
builder.Services.AddTransient<ExtractedStrings>();
builder.Services.AddTransient<ScoringAlg>(sp => new ScoringAlg(sp.GetRequiredService<IFileAnalysis>(),
    sp.GetRequiredService<HandleResult>(),
    0,
    sp.GetRequiredService<ExtractedStrings>()));

builder.Services.AddTransient<StaticFileAnalysis>(sp => new StaticFileAnalysis(sp.GetRequiredService<IFileAnalysis>(),
    sp.GetRequiredService<ScoringAlg>(),
    sp.GetRequiredService<ExtractedStrings>()));

builder.Services.AddTransient<IEmailServices, EmailServices>();
builder.Services.AddTransient<NewLetter>(sp => new NewLetter(sp.GetRequiredService<IEmailServices>()));
builder.Services.AddTransient<IAdminRepo, AdminRepository>(sp =>
    new AdminRepository(connetionString)
);*/


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


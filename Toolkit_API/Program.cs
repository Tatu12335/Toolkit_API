using Dapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text;
using System.Threading.RateLimiting;
using Toolkit_API.Application.Analysis;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.Application.Application_Services.FileOperations;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.Application.Calculators;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.FileAnalysis;
using Toolkit_API.Domain.Entities.Files;
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
var connectionStringHangfire = Environment.GetEnvironmentVariable("HANGFIRE") ?? throw new InvalidOperationException("'HANGFIRE' not found");

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
                PermitLimit = 100,
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
builder.Services.AddTransient<ZipPolicies>();
builder.Services.AddTransient<IZipHandler, HandleZip>();
builder.Services.AddTransient<HandleZip>();
builder.Services.AddTransient<HandleResult>();
builder.Services.AddTransient<FolderInfo>();
builder.Services.AddTransient<IHandleUploadFolder, HandleUploadFolder>();
builder.Services.AddTransient<IhangfireService, HangfireService>();
builder.Services.AddTransient<ICapabilityAnalyzer, CapabilityAnalyzer>();
builder.Services.AddTransient<Insert>();
builder.Services.AddTransient<Calculate_Risk_Level>();
builder.Services.AddTransient<IFileHasher, FileHasher>();
builder.Services.AddTransient<ScoringAlgorithmn>();
builder.Services.AddTransient<IImportAnalyzer, ImportAnalyzer>();
builder.Services.AddTransient<ConfidenceANDSeverityCalculator>();


builder.Services.AddTransient<CapabilityRuleset>(options => new CapabilityRuleset(
        Capability.None,
        ["-"]
    ));
builder.Services.AddTransient<IDetectionSourceBuilder, DetectionSourceBuilder>();

builder.Services.AddTransient<IDetectionSourceBuilder,DetectionSourceBuilder>(options => 
    new DetectionSourceBuilder(options.GetRequiredService<IFileAnalysis>())
);

builder.Services.AddTransient<IImportAnalyzer, ImportAnalyzer>(options => 
    new ImportAnalyzer(options.GetRequiredService<ICapabilityAnalyzer>())
    
);



builder.Services.AddTransient<IScan, ScanService>(options =>
    new ScanService(options.GetRequiredService<IResultRepository>(), options.GetRequiredService<StaticScan>())
);
builder.Services.AddTransient<IResultRepository, ResultRepository>(options =>
    new ResultRepository(connetionString)
);

builder.Services.AddTransient<HashOps>(options =>
    new HashOps(
        options.GetRequiredService<IFileHasher>(),
        options.GetRequiredService<IFileScanRepo>()

    )

);
builder.Services.AddTransient<StaticScan>(options =>
    new StaticScan(
        options.GetRequiredService<IFileScanRepo>(),
        options.GetRequiredService<HashOps>(),
        options.GetRequiredService<ICallExternalAPI>(),
        options.GetRequiredService<Calculate_Risk_Level>(),
        options.GetRequiredService<ICapabilityAnalyzer>(),
        options.GetRequiredService<ExtractedStrings>(),
        options.GetRequiredService<IFileAnalysis>(),
        options.GetRequiredService<IResultRepository>(),
        options.GetRequiredService<IDetectionSourceBuilder>(),
        options.GetRequiredService<ConfidenceANDSeverityCalculator>(),
        options.GetRequiredService<ScoringAlgorithmn>(),
        options.GetRequiredService<Insert>()
    )
);
builder.Services.AddHangfire(options =>
{
    options.UseSqlServerStorage(connectionStringHangfire);
});
builder.Services.AddHangfireServer();

builder.Services.AddTransient<Insert>(options =>
    new Insert(
    options.GetRequiredService<IFileScanRepo>()
    )
);


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

builder.Services.AddTransient<IFileAnalysis, FileAnalysis>(options =>
    new FileAnalysis(
        options.GetRequiredService<ICapabilityAnalyzer>(),
        options.GetRequiredService<IImportAnalyzer>()
    )
);


builder.Services.AddTransient<IFileScanRepo, FileScanRepo>(options =>
    new FileScanRepo(options.GetRequiredService<FileHasher>(),
    connetionString
    )
);
builder.Services.AddSignalR();


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

builder.Services.Configure<KestrelServerOptions>(options =>
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10)
);



var app = builder.Build();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( );
}
//app.MapHub<Scanhub>("/scanHub");

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();


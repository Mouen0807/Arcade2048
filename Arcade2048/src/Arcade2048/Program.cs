using Arcade2048.Databases;
using Arcade2048.Extensions.Application;
using Arcade2048.Extensions.Services;
using Arcade2048.Resources;
using Arcade2048.Resources.HangfireUtilities;
using Destructurama;
using Hangfire;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName)
    .Destructure.UsingAttributes()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (TraceId: {TraceId}){NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.ConfigureServices();

// 1. Get jwtSettings from config
var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings section is missing.");

// 2. Use jwtSettings in AddAuthentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using var scope = app.Services.CreateScope();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseProblemDetails();

// For elevated security, it is recommended to remove this middleware and set your server to only listen on https.
// A slightly less secure option would be to redirect http to 400, 505, etc.
app.UseHttpsRedirection();

app.UseCors("Arcade2048CorsPolicy");

app.MapHealthChecks("api/health");

app.Use(async (context, next) =>
{
    using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
    {
        await next();
    }
});

app.UseSerilogRequestLogging();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    AsyncAuthorization = new[] { new HangfireAuthorizationFilter(scope.ServiceProvider) },
    IgnoreAntiforgeryToken = true
});

app.UseSwaggerExtension(builder.Configuration, builder.Environment);

try
{
    Log.Information("Starting application");
    await app.RunAsync();
}
catch (Exception e)
{
    Log.Error(e, "The application failed to start correctly");
    throw;
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}

// Make the implicit Program class public so the functional test project can access it
public partial class Program { }
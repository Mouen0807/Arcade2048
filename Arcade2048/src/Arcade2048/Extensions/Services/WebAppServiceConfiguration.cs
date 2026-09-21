namespace Arcade2048.Extensions.Services;

using Arcade2048.Middleware;
using Arcade2048.Services;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Resources;
using Resources;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<Arcade2048Options>(builder.Configuration.GetSection(Arcade2048Options.SectionName));

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddProblemDetails(ProblemDetailsConfigurationExtension.ConfigureProblemDetails)
            .AddProblemDetailsConventions();

        // TODO update CORS for your env
        builder.Services.AddCorsService("Arcade2048CorsPolicy", builder.Environment);
        builder.OpenTelemetryRegistration(builder.Configuration, "Arcade2048");
        builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddApiVersioningExtension();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // registers all services that inherit from your base service interface - IArcade2048ScopedService
        builder.Services.AddBoundaryServices(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc();

        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension(builder.Configuration);
    }

    /// <summary>
    /// Registers all services in the assembly of the given interface.
    /// </summary>
    private static void AddBoundaryServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        foreach (var assembly in assemblies)
        {
            var rules = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IArcade2048ScopedService)) == typeof(IArcade2048ScopedService));

            foreach (var rule in rules)
            {
                foreach (var @interface in rule.GetInterfaces())
                {
                    services.Add(new ServiceDescriptor(@interface, rule, ServiceLifetime.Scoped));
                }
            }
        }
    }
}
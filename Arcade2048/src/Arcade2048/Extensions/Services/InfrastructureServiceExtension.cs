namespace Arcade2048.Extensions.Services;

using Arcade2048.Databases;
using Arcade2048.Resources;
using Arcade2048.Services;
using Arcade2048.Resources.HangfireUtilities;
using Resources;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        // DbContext -- Do Not Delete
        var connectionString = configuration.GetConnectionStringOptions().Arcade2048;
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            // this makes local migrations easier to manage. feel free to refactor if desired.
            connectionString = env.IsDevelopment() 
                ? "Data Source=localhost,63935;Integrated Security=False;Database=dev_arcade2048;User ID=SA;Password=#localDockerPassword#"
                : throw new Exception("The database connection string is not set.");
        }

        services.AddDbContext<Arcade2048DbContext>(options =>
            options.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(typeof(Arcade2048DbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());

        services.AddHostedService<MigrationHostedService<Arcade2048DbContext>>();

        services.SetupHangfire(env);

        // Auth -- Do Not Delete
    }
}
    
public static class HangfireConfig
{
    public static void SetupHangfire(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IJobContextAccessor, JobContextAccessor>();
        services.AddScoped<IJobWithUserContext, JobWithUserContext>();
        // if you want tags with sql server
        // var tagOptions = new TagsOptions() { TagsListStyle = TagsListStyle.Dropdown };
        
        // var hangfireConfig = new MemoryStorageOptions() { };
        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseMemoryStorage()
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                // if you want tags with sql server
                // .UseTagsWithSql(tagOptions, hangfireConfig)
                .UseActivator(new JobWithUserContextActivator(services.BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>()));
        });
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 10;
            options.ServerName = $"Arcade2048-{env.EnvironmentName}";

            if (Consts.HangfireQueues.List().Length > 0)
            {
                options.Queues = Consts.HangfireQueues.List();
            }
        });

    }
}

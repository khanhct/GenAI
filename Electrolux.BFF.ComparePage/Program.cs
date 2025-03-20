using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using T1.ReArch.BFF.Core;
using Electrolux.BFF.ComparePage.Middleware;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        // Add Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = context.Configuration.GetConnectionString("RedisConnection");
        });

        // Add HTTP Client
        services.AddHttpClient();

        // Add Core Services
        services.AddT1CoreServices(options =>
        {
            options.T1ApiBaseUrl = context.Configuration["T1Api:BaseUrl"];
            options.AuthApiBaseUrl = context.Configuration["AuthApi:BaseUrl"];
        });

        // Register Authorization Middleware
        services.AddScoped<AuthorizationMiddleware>();
    })
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<AuthorizationMiddleware>();
    })
    .Build();

await host.RunAsync(); 
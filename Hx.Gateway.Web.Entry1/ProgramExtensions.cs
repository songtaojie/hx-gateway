// Only use in this file to avoid conflicts with Microsoft.Extensions.Logging
using Hx.Shared.Dapr;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Serilog;

public static class ProgramExtensions
{ 

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder) =>
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDapr()
            .AddUrlGroup(new Uri(builder.Configuration["SysHC"]), name: "sysapi-check", tags: new string[] { "sysapi" })
            .AddUrlGroup(new Uri(builder.Configuration["demoHC"]), name: "demoapi-check", tags: new string[] { "demoapi" });
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <returns></returns>
    public static IHostBuilder AddOcelotJson(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((hostingContext, builder) =>
        {
            builder.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("ocelot.json", true, true)
                .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                //.AddOcelot(hostingContext.HostingEnvironment)
                .AddEnvironmentVariables();
        });
    }
}


 
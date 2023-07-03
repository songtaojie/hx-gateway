//using Microsoft.OpenApi.Models;
//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;

//var builder = WebApplication.CreateBuilder(args);
////builder.AddCustomHealthChecks();
//builder.Host.AddOcelotJson();
//builder.Services.AddCorsAccessor(builder.Configuration);
//builder.Services.AddMvc();
//builder.Services.AddOcelot();

//var app = builder.Build();

//app.UseCorsAccessor();
//app.UseOcelot().Wait();
//app.MapGet("/", context => {
//    context.Response.Redirect("/swagger");
//    return Task.CompletedTask;
//});

//app.Run();

Serve.Run(RunOptions.Default.AddWebComponent<WebComponent>());
public class WebComponent : IWebComponent
{
    public void Load(WebApplicationBuilder builder, ComponentContext componentContext)
    {
        // ÈÕÖ¾¹ýÂË
        builder.Logging.AddFilter((provider, category, logLevel) =>
        {
            return !new[] { "Microsoft.Hosting", "Microsoft.AspNetCore" }.Any(u => category.StartsWith(u)) && logLevel >= LogLevel.Information;
        });
    }
}



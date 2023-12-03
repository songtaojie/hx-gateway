using AntDesign.ProLayout;
using Hx.Gateway.Admin.Services;
using Hx.Gateway.Application.Services;
using Hx.Gateway.Core.Options;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers(); ;
builder.Services.AddSqlSugar(builder.Configuration);
builder.Services.AddGatewayServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

//app.UseBlazorFrameworkFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Run();

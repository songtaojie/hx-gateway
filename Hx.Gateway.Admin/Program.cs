// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<UnifyResultFilterAttribute>();
    options.Filters.Add<FriendlyExceptionFilter>();
});
builder.Services.AddSwaggerGen();
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddSqlSugar(builder.Configuration);
builder.Services.AddGatewayServices();
builder.Services.AddCorsAccessor(builder.Configuration);
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
 
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // using static System.Net.Mime.MediaTypeNames;
            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("An exception was thrown.");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync(" The file was not found.");
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                await context.Response.WriteAsync(" Page: Home.");
            }
        });
    });

    app.UseHsts();
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCorsAccessor();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

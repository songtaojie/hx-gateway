// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole()
    .AddConsoleFormatter<ConsoleFormatterExtend, ConsoleFormatterOptions>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UnifyResultFilterAttribute>();
    options.Filters.Add<FriendlyExceptionFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options => 
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter());
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName);
});
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddSqlSugar(builder.Configuration);
builder.Services.AddGatewayServices();
builder.Services.AddCorsAccessor(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseCorsAccessor();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

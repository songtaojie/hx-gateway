// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FriendlyExceptionFilter>();
    options.Filters.Add<UnifyResultFilterAttribute>();
});
builder.Services.AddSwaggerGen();
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

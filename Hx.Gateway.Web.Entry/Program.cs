
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.ConfigureHxWebApp();
builder.Services.AddHxHttpClient();
builder.Services.AddHxOcelot(builder.Configuration);
//builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseHxOcelot().Wait();
//app.UseOcelot().Wait();
app.UseAuthorization();

app.MapControllers();

app.Run();

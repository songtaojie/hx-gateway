// Apache-2.0 License
// Copyright (c) 2021-2022 
// ����:songtaojie
// �绰/΢�ţ�stjworkemail@163.com

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

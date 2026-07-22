using Microsoft.AspNetCore.Builder;

//using pos.lib;
using Microsoft.Extensions.DependencyInjection;
using demo_project.api;
using demo_project.app;
using demo_project.repository;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//builder.Services.AddHostingSwagger();

builder.Services.AddMiniCdpApi(configuration);
builder.Services.AddZaloAppRepository(configuration);
builder.Services.AddMiniFormAppService(configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://h5.zdn.vn", "zbrowser://h5.zdn.vn", "https://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();  // Cho phép gửi cookie nếu cần
    });
});
var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.AddZaloAppApplicationBuilder();
//app.UseHostingSwagger();

app.Run();

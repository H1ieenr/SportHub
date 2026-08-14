using sporthub.repository;
using sporthub.app;
using sporthub.domain;
using Shared.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSportHubRepository(builder.Configuration);
builder.Services.AddAppService(builder.Configuration);
builder.Services.AddSharedAuthentication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SportHubDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Users>>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedAsync(context, hasher);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

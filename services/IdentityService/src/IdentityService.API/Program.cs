using IdentityService.Infrastructure.Extensions;
using IdentityService.API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
//add and validate options at startup
builder.Services.AddOptions(builder.Configuration);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration.GetJwtOptions());
builder.Services.AddAuthorization();
builder.Services.AddIdentityInfrastructure(builder.Configuration.GetDatabaseOptions());

builder.Services.AddApplicationServices(ServiceLifetime.Scoped);
builder.Services.AddRepositories(ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Theme = ScalarTheme.BluePlanet;
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

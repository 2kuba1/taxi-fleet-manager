using API.Endpoints;
using Application;
using Infrastructure;
using Persistence;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Layer services
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();

//Auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.SeedDatabaseAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();
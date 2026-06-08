using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Layer services
builder.Services.AddInfrastructureService(builder.Configuration);

//Auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
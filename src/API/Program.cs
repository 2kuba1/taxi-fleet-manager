using System.Text;
using API.Endpoints;
using Application;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Layer services
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();

//Auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });

//Cors
builder.Services.AddCors(opt =>
{
    var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();
    
    opt.AddPolicy("WebAppCorsPolicy", policy =>
    {
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

await app.SeedDatabaseAsync();

app.UseHttpsRedirection();

app.UseCors("WebAppCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();
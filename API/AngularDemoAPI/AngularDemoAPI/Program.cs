using AngularDemoAPI.Configuration;
using AngularDemoAPI.Data;
using AngularDemoAPI.Hubs;
using AngularDemoAPI.Middlewares;
using AngularDemoAPI.Models.Domain;
using AngularDemoAPI.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System;
using System.Text;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

if(!builder.Environment.IsDevelopment())
{
    var keyVaultUri = new Uri($"https://school-mgmt-kv.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
}

// Controllers
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// OpenAPI (Microsoft)
builder.Services.AddOpenApi();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException( "JWT signing key is not configured. Set the 'Jwt__Key' environment variable.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Auth failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddCustomServices();

// DbContext
builder.Services.AddDbContext<AngularDemoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// OpenAPI + Scalar
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference();
//}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MeetingHub>("/meetinghub");

// Redirect root Scalar
app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.Run();
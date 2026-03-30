using BlogService.Data;
using Microsoft.EntityFrameworkCore;
using BlogService.AsyncDataServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using BlogService.EventProcessing;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Database Context
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("DevelopmentDb"));

// Repositories for database access
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IBlogRepo, BlogRepo>();

// Event Processor
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

// Message bus background listener
builder.Services.AddHostedService<MessageBusSubscriber>();

// AutoMapper (Mapping between DTOs and Models)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure authentication
// Setup RSA for asymetric token encryption
var Rsa = RSA.Create();
Rsa.ImportFromPem(builder.Configuration["Jwt:PublicKey"]!);

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
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(Rsa),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

// Add authorization service
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Configure controller Endpoints
app.MapControllers();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Seed database
//PrepDb.PrepPopulation(app);

app.Run();
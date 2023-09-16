using System.Text;
using System.Text.Json.Serialization;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;

using DotNetEnv;
using DotNetEnv.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
var configuration = builder.Configuration.GetConnectionString("DefaultConnection");
var reactServerUrl = builder.Configuration["ReactServerUrl"];

#endregion

#region Services

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration));

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Controllers with enum string convert option
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy =>
            policy.WithOrigins(reactServerUrl).AllowAnyMethod().AllowAnyHeader()));

// JWT Authentication/Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// Other services
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IMediaManager, LocalMediaManager>();

var app = builder.Build();

#endregion

#region Seed Data

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    using SeedData seedData = new(serviceProvider);
    seedData.Initialize();
}

#endregion

#region Middlewares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion

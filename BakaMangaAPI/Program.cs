using System.Text;
using System.Text.Json.Serialization;

using BakaMangaAPI.Data;
using BakaMangaAPI.Models;
using BakaMangaAPI.Services.Media;
using BakaMangaAPI.Services.Notification;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("JWT Bearer Auth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT Bearer Auth"
                }
            },
            new string[] {}
        }
    });
});

// Cors
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy =>
            policy.WithOrigins(reactServerUrl).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

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

// SignalR for Notification
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
builder.Services.AddTransient<INotificationManager, NotificationManager>();

// Other services
builder.Services.AddAutoMapper(typeof(Program));
if (builder.Environment.IsProduction())
{
    builder.Services.AddScoped<IMediaManager, CloudinaryMediaManager>();
}
else
{
    builder.Services.AddScoped<IMediaManager, LocalMediaManager>();
}

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

if (!app.Environment.IsProduction())
{
    app.UseStaticFiles();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notify");

app.Run();

#endregion

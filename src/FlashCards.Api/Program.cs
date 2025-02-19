using System;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Domain.Entities;
using FlashCards.Infrastructure.Persistence.Contexts;
using FlashCards.Infrastructure.Persistence.Repositories;
using FlashCards.Infrastructure.Security;
using FlashCards.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7232") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); 
    });
});
// JWT configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
// Services
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtProvider>();
// Authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, //mb change in future
            ValidateAudience = false, //mb change in future
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()?.Key))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["AccessToken"];
                Console.WriteLine(context.Token); // TODO only for debug, using logger instead of
                return Task.CompletedTask;
            }
        };
    });
// Authorization 
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cookie configuration
app.UseCookiePolicy(new CookiePolicyOptions // TODO: Cookie expires time? `Expires = DateTimeOffset.UtcNow.AddDays(7)`
{
    MinimumSameSitePolicy = SameSiteMode.None,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
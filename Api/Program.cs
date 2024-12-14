
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extension;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Features.Queries.Services.Implementation;
using Hangfire;
using Application.Features.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Google;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("QuizMasterAi");
            
            builder.Services.AddControllers();

            builder.Services.AddDbContext<QuizMasterAiDb>(opt =>
            opt.UseNpgsql(connection));
            builder.Services.AddMediatRs();
            
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<QuizMasterAiDb>()
                .AddDefaultTokenProviders();
            builder.Services.AddRepository();
            builder.Services.AddService();
            builder.Services.AddScoped<HttpClient>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddHangfire(config =>
            {
                config.UseStorage(new PostgreSqlStorage(connection, new PostgreSqlStorageOptions
                {
                    // You can set options here if needed
                    QueuePollInterval = TimeSpan.FromSeconds(10)
                }));
            });
            builder.Services.AddHangfireServer();

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            var configuration = builder.Configuration;

           

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(30),

                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            })
            .AddGoogle(googleOptions => 
            {
                googleOptions.ClientId = configuration["Google:ClientId"] ?? throw new NullReferenceException();
                googleOptions.ClientSecret = configuration["Google:ClientSecret"] ?? throw new NullReferenceException();
                googleOptions.CallbackPath = "/google-response"; // Match to Google Console
                //googleOptions.SignInScheme = JwtBearerDefaults.AuthenticationScheme; //JWT instead of cookies
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Configure JWT Bearer authentication for Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
            });
            //builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7164")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRole.SeedRoles(roleManager);
            }

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            

            app.UseHangfireDashboard();

            app.UseHangfireServer();
            app.MapControllers();

            app.Run();
        }
    }
}

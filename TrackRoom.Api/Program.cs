using BrainHope.Services.DTO.Email;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using TrackRoom.Api.Hubs;
using TrackRoom.DataAccess;
using TrackRoom.DataAccess.Contexts;
using TrackRoom.DataAccess.Models;
using TrackRoom.Infrastructure;
using TrackRoom.Services;
using TrackRoom.Services.Interfaces;
using TrackRoom.Services.Services;
using Utilites;

namespace TrackRoom.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Inject external DI
            builder.Services
                .AddDataAccessServices(config)
                .AddServiceLayer()
                .AddApiLayer(config);

            // Identity config
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #region JWT


            #region Cores

            // 🔹 Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            #endregion


            // ✅ Add infrastructure layer (JWT, Google)
            builder.Services.AddInfrastructure(builder.Configuration);
            #endregion


            #region Email
            var Configure = builder.Configuration;
            var emailconfig = Configure.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailconfig);
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);
            #endregion
            builder.Services.AddAuthorization();

            // Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = config.GetSection("Redis")["ConnectionString"];
                return ConnectionMultiplexer.Connect(redisConnection);
            });

            builder.Services.AddScoped<IMeetingService, MeetingService>();


            builder.Services.AddSignalR();

            var app = builder.Build();

            // Redis ImageHelper Config
            var accessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            ImageHelper.Configure(accessor, env);

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.MapHub<CallHub>("/callhub");

            app.Run();
        }
    }
}

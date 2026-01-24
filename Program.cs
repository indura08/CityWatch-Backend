
using cityWatch_Project.Data;
using cityWatch_Project.Helpers;
using cityWatch_Project.Repositories.Implementations;
using cityWatch_Project.Repositories.Interfaces;
using cityWatch_Project.Services.Implementations;
using cityWatch_Project.Services.Interfaces;
using cityWatch_Project.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cityWatch_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<MainDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //services classes registration
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositroy>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<JwtTokenGenerator>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositroy>();

            //adding jwt configuration
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt")
            );

            //methna allow all deela thibbata eka podkd hnna one wenama cors allowed clients lata wihthrk requests ewanna puluwan wena widiyt
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAllOrigins",

                    policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyOrigin();
                        policy.AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

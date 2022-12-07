using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Tickets.Aplication.Interfaces.Account;
using Tickets.Aplication.Models;
using Tickets.Domain.Settings;
using Tickets.Infraestructure.Identity.Models;
using Tickets.Infraestructure.Identity.Services;

namespace Tickets.Infraestructure.Identity.Extensions
{
    public static class ServiceJWTExtension
    {
        public static IServiceCollection AddIdentityInfraestructure(this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddDbContext<TicketDBContext>(x =>
               x.UseSqlServer(configurationManager.GetConnectionString("Identity")));
            #region Persistence
            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<TicketDBContext>().AddDefaultTokenProviders();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            #endregion

            #region  JWT
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            services.Configure<JwtSettings>(configurationManager.GetSection("JwtSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = configurationManager["JwtSettings:Issuer"],
                     ValidAudience = configurationManager["JwtSettings:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationManager["JwtSettings:SecretKey"]))
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         var result = JsonSerializer.Serialize(new ResponseModel<string>(message: "You are not authorized to access the application"));
                         return context.Response.WriteAsync(result);
                     },
                     OnForbidden = context =>
                     {
                         context.Response.StatusCode = 403;
                         context.Response.ContentType = "application/json";
                         var result = JsonSerializer.Serialize(new ResponseModel<string>(message: "You are not authorized to access this resource"));
                         return context.Response.WriteAsync(result);
                     },
                 };
             });

            #endregion

            #region Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            return services;
        }
    }
}
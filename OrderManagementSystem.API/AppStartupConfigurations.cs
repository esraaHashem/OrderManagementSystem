﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Application.Order;
using OrderManagementSystem.Infrastructure.Data;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace OrderManagementSystem.API
{
    /// <summary>
    /// created custom extension methods for configuration DI services and keep program.cs clean
    /// </summary>
    public static class AppStartupConfigurations
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscountStrategy, NewCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, LoyalCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, RegularCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, VIPCustomerDiscountStrategy>();

            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        /// <summary>
        /// enable swagger to read summeries written above classes.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlComments));
            });

            return services;
        }

        /// <summary>
        /// inject the dbcontext.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppDBContext(this IServiceCollection services)
        {
            services.AddDbContext<OrderManagementDBContext>(options =>
            {
                options.UseInMemoryDatabase("OrderManagementSystemDB");
            });
            return services;
        }

        /// <summary>
        /// created a global exception handler that can have custom exceptions returned to users.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppCustomExceptionHandler(this IServiceCollection services)
        {
            services.AddProblemDetails();
            services.AddExceptionHandler<AppCustomExceptionHandler>();
            return services;
        }

        /// <summary>
        /// to protect endpoints added authentication using jwt and injected/configured it.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddJWTAuthentication(this WebApplicationBuilder builder)
        {
            var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                };
            });

            return builder.Services;
        }

        /// <summary>
        /// For prevent apis from abuse rate limiting is added
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRateLimiting(this IServiceCollection services)
        {
            //configure rate limiting here permit 10 requests within 30 sec more if user hits 11 requests with the 30 sec, response 503 is returned
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 0,
                            Window = TimeSpan.FromSeconds(30)
                        }));
            });

            return services;
        }
    }
}
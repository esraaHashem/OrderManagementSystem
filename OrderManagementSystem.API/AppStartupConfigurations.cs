using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Application.Order;
using OrderManagementSystem.Infrastructure.Data;
using System.Reflection;
using System.Text;

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
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscountStrategy, NewCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, LoyalCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, RegularCustomerDiscountStrategy>();

            services.AddScoped<IDiscountStrategy, VIPCustomerDiscountStrategy>();

            services.AddSingleton<IOrderService, OrderService>();

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
        public static IServiceCollection InjectJWTAuthentication(this WebApplicationBuilder builder)
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
    }
}
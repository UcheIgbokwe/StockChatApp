using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.EventBusConsumer;
using Domain.Interface;
using Domain.Interface.Repository;
using EventBus.Messages.Events;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ChatConnectionString"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });
            services.AddScoped<GetStockConsumer>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStocksService, StocksService>();
            services.AddHttpClient<StocksService>("stocksService");

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
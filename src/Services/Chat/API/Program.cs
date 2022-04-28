using API.Extensions;
using API.EventBusConsumer;
using API.Helper;
using Infrastructure.CronJob;
using Infrastructure.Data;
using Infrastructure.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventBus.Messages.Common;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
// Add services to the container.
{
    var services = builder.Services;

    services.AddCors();
    services.AddScoped<IMyScopedService, MyScopedService>();
    services.AddCronJob<MyCronJob1>(c =>
    {
        c.TimeZoneInfo = TimeZoneInfo.Local;
        c.CronExpression = "*/1 * * * *";
    });

    services.AddScoped<GetStockConsumer>();
    //Mass transit-RabbitMq Config
    services.AddMassTransit(config => {
        config.AddConsumer<GetStockConsumer>();

        config.UsingRabbitMq((ctx, cfg) => {
            cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
            cfg.ReceiveEndpoint(EventBusConstants.GetStockQueue, c => c.ConfigureConsumer<GetStockConsumer>(ctx));
        });
    });
    services.AddMassTransitHostedService();
    services.AddControllers().AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
    services.AddAutoMapper(typeof(Program));

    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.AddDbContext<ChatDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ChatConnectionString"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSwaggerDocumentation();
    services.AddAuthorizationServices(builder.Configuration);
    services.AddApplicationServices();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();

using API.Extensions;
using API.EventBusConsumer;
using API.Helper;
using Infrastructure.CronJob;
using MassTransit;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using System.Reflection;

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

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSwaggerDocumentation();
    services.AddAuthorizationServices(builder.Configuration);
    services.AddApplicationServices(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();

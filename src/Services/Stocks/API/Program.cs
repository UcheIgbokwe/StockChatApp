using API.Extensions;
using EventBus.Messages.Common;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
{
    var services = builder.Services;

    services.AddCors(p => p.AddPolicy("corsapp", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()));

    //Mass transit-RabbitMq Config
    services.AddMassTransit(config => config.UsingRabbitMq((ctx, cfg) => cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"))));
    services.AddMassTransitHostedService();
    services.AddControllers().AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSwaggerDocumentation();
    services.AddApplicationServices();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("corsapp");

app.MapControllers();

app.Run();

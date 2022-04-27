
var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
{
    var services = builder.Services;

    services.AddCors(p => p.AddPolicy("corsapp", builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()));
    services.AddControllers().AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("corsapp");

app.MapControllers();

app.Run();

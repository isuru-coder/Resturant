using Resturant.Application.Extensions;
using Resturant.Infrastructure.Extensions;
using Resturant.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Host.UseSerilog((context, configeration) =>
//    configeration
//        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
//        .WriteTo.File("Logs/Resturant-API.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
//        .WriteTo.Console(outputTemplate: "[{Timestamp: dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}")
//);

builder.Host.UseSerilog((context, configeration) =>
    configeration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IResturantSeeder>();

await seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
